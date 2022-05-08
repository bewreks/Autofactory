using System;
using System.Collections.Generic;
using System.Linq;
using Factories;
using Installers;
using Zenject;

namespace Inventories
{
	public class Inventory : IInventory
	{
		[Inject] private InventoryPacksModelsSettings _settings;

		private InventoryTypesModel _model;

		private Dictionary<InventoryObjectsTypesEnum, FullInventoryPack> _packs =
			new Dictionary<InventoryObjectsTypesEnum, FullInventoryPack>();

		public void Initialize(InventoryTypesEnum type)
		{
			_model = _settings.GetInventoryModel(type);
		}

		[Inject]
		public void Construct()
		{
			Initialize(InventoryTypesEnum.UNLIMITED);
		}

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

		public int FreePacksCount()
		{
			var packsCount = _packs.Values.SelectMany(pack => pack.Packs).Count();
			return (int)(_model.Limit - packsCount);
		}

		public FullInventoryPack GetPacks(InventoryObjectsTypesEnum type)
		{
			return _packs.TryGetValue(type, out var value) ? value : null;
		}

		public bool AddItems(InventoryObjectsTypesEnum type, int count, out int edge)
		{
			edge = 0;
			if (count < 0)
			{
				return false;
			}
			
			if (!SearchPack(type,   out var value)) return false;
			
			var freePacksCount = FreePacksCount();

			edge = value.Add(count, freePacksCount);
			return true;
		}

		public bool AddItems(InventoryPack pack, out int edge)
		{
			edge = 0;
			if (SearchPack(pack.Model.Type, out var value))
			{
				var freePacksCount = FreePacksCount();

				edge = value.Add(pack, freePacksCount);
				return true;
			}

			return false;
		}

		public bool AddItems(FullInventoryPack packs, out int edge)
		{
			edge = 0;
			if (SearchPack(packs.Model.Type, out var value))
			{
				var freePacksCount = FreePacksCount();
				
				edge = value.Add(packs, freePacksCount);
				return true;
			}

			return false;
		}

		public bool RemoveItem(InventoryObjectsTypesEnum type, int count = 1)
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

		public int ItemsCount(InventoryObjectsTypesEnum type)
		{
			if (SearchPack(type, out var value))
			{
				return value.Count.Value;
			}

			return 0;
		}

		private bool SearchPack(InventoryObjectsTypesEnum type, out FullInventoryPack value)
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