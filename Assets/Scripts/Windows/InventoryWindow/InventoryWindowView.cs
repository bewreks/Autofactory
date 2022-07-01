using System.Collections.Generic;
using Inventories;
using UnityEngine;
using UnityEngine.UI;

namespace Windows.InventoryWindow
{
	public class InventoryWindowView : WindowView
	{
		[SerializeField] private InventoryPackView inventoryPackPrefab;
		[SerializeField] private Button            closeButton;
		[SerializeField] private Transform         inventoryContent;

		private List<InventoryPackView> _packViews = new List<InventoryPackView>();
		public  InventoryPackView       InventoryPackPrefab => inventoryPackPrefab;


		public override void Opening(float duration) { }

		public override void Closing(float duration) { }

		public override void Hiding(float duration) { }

		public override void AfterOpen()
		{
			closeButton.onClick.AddListener(CastOnClose);
		}

		public override void BeforeClose()
		{
			_packViews.ForEach(view => view.Dispose());
			closeButton.onClick.RemoveAllListeners();
		}

		public void ShowPacks(IReadOnlyList<InventoryPack> inventoryPacks, InventoryPackView prefab)
		{
			foreach (var pack in inventoryPacks)
			{
				var packView = Instantiate(prefab,
				                           Vector3.zero,
				                           Quaternion.identity,
				                           inventoryContent).GetComponent<InventoryPackView>();
				packView.SetData(pack);
				_packViews.Add(packView);
			}
		}
	}
}