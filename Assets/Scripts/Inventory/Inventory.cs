using System;
using System.Collections.Generic;
using System.Linq;
using Factories;
using UnityEngine;
using Zenject;

namespace Inventories
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
			return SearchPack(type, out var value) && value.Add(count) == 0;
		}

		public bool AddItems(InventoryPack pack)
		{
			if (SearchPack(pack.Model.Type, out var value))
			{
				return value.Add(pack) == 0;
			}

			return false;
		}

		public bool AddItems(FullInventoryPack packs)
		{
			if (SearchPack(packs.Model.Type, out var value))
			{
				return value.Add(packs) == 0;
			}

			return false;
		}

		public bool RemoveItem(InventoryTypesEnum type, int count = 1)
		{
			if (!SearchPack(type, out var value)) return false;
			return value.Remove(count) == 0;
		}

		public bool RemoveItem(InventoryPack pack)
		{
			var type = pack.Model.Type;
			if (!SearchPack(type, out var value)) return false;
			var removeResult = value.Remove(pack) == 0;
			return removeResult;
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
			if (SearchPack(type, out var value))
			{
				return value.Count.Value;
			}

			return 0;
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
				catch (Exception e)
				{
					Debug.LogError(e.Message);
					return false;
				}
			}

			return true;
		}
	}
}