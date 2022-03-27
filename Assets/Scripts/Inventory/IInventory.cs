using System;
using System.Collections;
using System.Collections.Generic;

namespace Inventory
{
	public interface IInventory : IDisposable
	{
		bool                AddItem(InventoryTypesEnum    type);
		bool                RemoveItem(InventoryTypesEnum type);
		List<InventoryPack> GetItems();
	}
}