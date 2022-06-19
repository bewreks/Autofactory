using System.Collections.Generic;
using Game;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using Zenject;

namespace Windows.InventoryWindow
{
	public class InventoryWindow : WindowView
	{
		[SerializeField] private InventoryPackView inventoryPackPrefab;
		[SerializeField] private Button            closeButton;
		[SerializeField] private Transform         inventoryContent;

		[Inject] private DiContainer        _container;
		[Inject] private IGameModel         _gameModel;

		private List<InventoryPackView> _packViews = new List<InventoryPackView>();

		public override void Opening()
		{
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

			closeButton.onClick.AddListener(CastOnClose);
			CastOnOpened();
		}

		public override void Closing()
		{
			_packViews.ForEach(view => view.Dispose());
			closeButton.onClick.RemoveAllListeners();
			CastOnClosed();
		}

		public override void Hiding()
		{
			_packViews.ForEach(view => view.Dispose());
			closeButton.onClick.RemoveAllListeners();
			CastOnHided();
		}
	}
}