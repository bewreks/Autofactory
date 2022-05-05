using System;
using System.Collections.Generic;

namespace Inventories
{
	public interface IInventory : IDisposable
	{
		public void Initialize(InventoryTypesEnum type);

		IReadOnlyList<InventoryPack> GetPacks();

		FullInventoryPack GetPacks(InventoryObjectsTypesEnum type);

		bool AddItems(InventoryObjectsTypesEnum type, int count, out int edge);

		bool AddItems(InventoryPack pack, out int edge);

		bool AddItems(FullInventoryPack packs, out int edge);

		bool RemoveItem(InventoryObjectsTypesEnum type, int count = 1);

		bool RemoveItem(InventoryPack pack);

		bool RemoveItem(FullInventoryPack packs);

		int ItemsCount(InventoryObjectsTypesEnum type);
	}
}