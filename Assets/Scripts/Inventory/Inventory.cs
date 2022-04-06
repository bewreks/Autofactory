using System;
using System.Collections.Generic;
using System.Linq;
using Factories;
using Zenject;

namespace Inventory
{
	public class Inventory : IInventory
	{
		[Inject] private InventoryPacksModelsManager _inventoryPacksModelsManager;
		
		private List<InventoryPack> _packs;

		public Inventory()
		{
			_packs = new List<InventoryPack>();
		}

		public bool AddItem(InventoryTypesEnum type)
		{
			var pack = _packs.FirstOrDefault(pack => pack.Type == type && !pack.IsFull);
			if (pack == null)
			{
				try
				{
					pack = Factory.GetFactoryItem<InventoryPack>();
					pack.Initialize(_inventoryPacksModelsManager.GetModel(type));
					_packs.Add(pack);
				}
				catch (Exception)
				{
					return false;
				}
			}
			else
			{
				return pack.AddItem();
			}

			return true;
		}

		public bool RemoveItem(InventoryTypesEnum type)
		{
			var pack = _packs.FirstOrDefault(pack => pack.Type == type && !pack.IsEmpty);

			if (pack == null) return false;

			var removeItemResult = pack.RemoveItem();

			if (pack.IsEmpty)
			{
				pack.Reset();
				_packs.Remove(pack);
				Factory.ReturnItem(pack);
			}
			
			return removeItemResult;
		}

		public void Dispose()
		{
			_packs.ForEach(pack => pack.Reset());
		}

		public List<InventoryPack> GetItems()
		{
			return _packs;
		}

		public int GetItemsCount(InventoryTypesEnum type)
		{
			var packs = _packs.Where(pack => pack.Type == type && !pack.IsFull);
			return packs.Sum(inventoryPack => inventoryPack.Size);
		}

		// TODO: Add full pack
		public InventoryPack GetPack(InventoryTypesEnum type)
		{
			var inventoryPacks = _packs.Where(pack => pack.Type == type);
			var pack           = inventoryPacks.OrderBy(pack => pack.Size).FirstOrDefault();
			return pack;
		}
	}
}