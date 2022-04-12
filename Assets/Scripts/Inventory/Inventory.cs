using System;
using System.Collections.Generic;
using System.Linq;
using Factories;
using Zenject;

namespace Inventory
{
	public class Inventory : IInventory
	{
		[Inject] private InventoryPacksModelsSettings _settings;

		private Dictionary<InventoryTypesEnum, FullInventoryPack> _packs =
			new Dictionary<InventoryTypesEnum, FullInventoryPack>();

		public void Dispose()
		{
			foreach (var fullInventoryPack in _packs.Values)
			{
				fullInventoryPack.Dispose();
			}
		}

		public IReadOnlyList<InventoryPack> GetPacks()
		{
			var inventoryPacks = _packs.Values.SelectMany(pack => pack.Packs);
			return inventoryPacks.ToList();
		}

		public FullInventoryPack GetPacks(InventoryTypesEnum type)
		{
			return _packs.TryGetValue(type, out var value) ? value : null;
		}

		public bool AddItems(InventoryTypesEnum type, int count = 1)
		{
			return SearchPack(type, out var value) && value.Add(count);
		}

		public bool AddItems(InventoryPack pack)
		{
			return SearchPack(pack.Model.Type, out var value) && value.Add(pack);
		}

		public bool AddItems(FullInventoryPack packs)
		{
			return SearchPack(packs.Model.Type, out var value) && value.Add(packs);
		}

		public bool RemoveItem(InventoryTypesEnum type, int count = 1)
		{
			return SearchPack(type, out var value) && value.Remove(count);
		}

		public bool RemoveItem(InventoryPack pack)
		{
			return SearchPack(pack.Model.Type, out var value) && value.Remove(pack);
		}

		public bool RemoveItem(FullInventoryPack packs)
		{
			SearchPack(packs.Model.Type, out var value);
			if (packs.Equals(value))
			{
				packs.Clear();
				return true;
			}

			return false;
		}

		public int ItemsCount(InventoryTypesEnum type)
		{
			return SearchPack(type, out var value) ? value.Count.Value : 0;
		}

		private bool SearchPack(InventoryTypesEnum type, out FullInventoryPack value)
		{
			if (!_packs.TryGetValue(type, out value))
			{
				try
				{
					value = Factory.GetFactoryItem<FullInventoryPack>();
					value.Initialize(null, _settings.GetModel(type));
					_packs.Add(type, value);
				}
				catch (Exception)
				{
					return false;
				}
			}

			return true;
		}
	}
}