using Inventories;
using UnityEngine;

namespace Windows.InventoryWindow
{
	[CreateAssetMenu(fileName = "InventoryWindow", menuName = "Models/Windows/InventoryWindow")]
	public class InventoryWindow : Window
	{
		protected override IWindowController CreateWindowController()
		{
			return new InventoryWindowController(_view, Data);
		}
	}

	internal class InventoryWindowController : WindowController
	{
		public InventoryWindowController(WindowView view, Window.WindowData data) : base(view, data) { }

		public override void PrepareView()
		{
			var view = (InventoryWindowView)_view;
			var data = (InventoryWindowData)_data;
			view.InventoryPackPrefab.gameObject.SetActive(true);
			view.ShowPacks(data.Inventory.GetPacks(), view.InventoryPackPrefab);
			view.InventoryPackPrefab.gameObject.SetActive(false);
		}
	}

	public class InventoryWindowData : Window.WindowData
	{
		public IInventory Inventory;
	}
}