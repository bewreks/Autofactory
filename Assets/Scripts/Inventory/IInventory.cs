using System;
using System.Collections.Generic;

namespace Inventory
{
	public interface IInventory : IDisposable
	{
		IReadOnlyList<InventoryPack> GetPacks();
   
		FullInventoryPack GetPacks(InventoryTypesEnum type);
   
		bool AddItems(InventoryTypesEnum type, int count = 1);
   
		bool AddItems(InventoryPack pack);
   
		bool AddItems(FullInventoryPack packs);
   
		bool RemoveItem(InventoryTypesEnum type, int count = 1);
   
		bool RemoveItem(InventoryPack pack);
   
		bool RemoveItem(FullInventoryPack packs);
   
		int ItemsCount(InventoryTypesEnum type);

	}
}