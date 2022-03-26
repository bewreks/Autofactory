using System;

namespace Inventory
{
	public interface IInventory : IDisposable
	{
		bool AddItem(InventoryTypesEnum type);
		bool RemoveItem(InventoryTypesEnum type);
	}
}