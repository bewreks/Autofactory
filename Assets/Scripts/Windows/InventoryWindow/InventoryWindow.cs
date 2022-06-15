using System.Collections.Generic;
using Game;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using Zenject;

namespace Windows.InventoryWindow
{
	public class InventoryWindow : Window
	{
		[SerializeField] private InventoryPackView inventoryPackPrefab;
		[SerializeField] private Button            closeButton;
		[SerializeField] private Transform         inventoryContent;

		[Inject] private DiContainer        _container;
		[Inject] private IGameModel         _gameModel;

		private List<InventoryPackView> _packViews = new List<InventoryPackView>();

		protected override void Opening()
		{
			_playerInputController.Player.InventoryWindows.performed += ClickInventoryWindowButton;
			var inventoryPacks = _gameModel.PlayerModel.Inventory.GetPacks();
			inventoryPackPrefab.gameObject.SetActive(true);

			foreach (var pack in inventoryPacks)
			{
				var packView = _container.InstantiatePrefab(inventoryPackPrefab,
				                                            Vector3.zero,
				                                            Quaternion.identity,
				                                            inventoryContent).GetComponent<InventoryPackView>();
				packView.SetData(pack);
				_packViews.Add(packView);
			}

			inventoryPackPrefab.gameObject.SetActive(false);

			closeButton.onClick.AddListener(Close);
			Opened();
		}

		private void ClickInventoryWindowButton(InputAction.CallbackContext obj)
		{
			_playerInputController.Player.InventoryWindows.performed -= ClickInventoryWindowButton;
			Close();
		}

		protected override void Closing()
		{
			_packViews.ForEach(view => view.Dispose());
			closeButton.onClick.RemoveAllListeners();
			Closed();
		}
	}
}