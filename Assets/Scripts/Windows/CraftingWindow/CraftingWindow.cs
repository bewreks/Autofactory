using System.Collections.Generic;
using Windows.InventoryWindow;
using Crafting;
using Game;
using Installers;
using Inventories;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using Zenject;

namespace Windows.CraftingWindow
{
	public class CraftingWindow : Window
	{
		[SerializeField] private CraftItemView  craftItemPrefab;
		[SerializeField] private Button         closeButton;
		[SerializeField] private Transform      craftContent;
		[SerializeField] private InventoryPopup inventoryPopup;

		[Inject] private DiContainer        _container;
		[Inject] private CraftSettings      _craftSettings;
		[Inject] private IGameModel         _gameModel;
		[Inject] private CraftingController _craftingController;

		private List<CraftItemView> _packViews = new List<CraftItemView>();

		protected override void Opening()
		{
			craftItemPrefab.gameObject.SetActive(true);

			foreach (var craftingModel in _craftSettings.GetModels)
			{
				var packView = _container.InstantiatePrefab(craftItemPrefab,
				                                            Vector3.zero,
				                                            Quaternion.identity,
				                                            craftContent).GetComponent<CraftItemView>();
				packView.SetData(craftingModel.CraftingResult);
				packView.OnMouseOver  += PackViewOnOnMouseOver;
				packView.OnMouseExit  += PackViewOnOnMouseExit;
				packView.OnMouseClick += PackViewOnOnMouseClick;
				_packViews.Add(packView);
			}

			craftItemPrefab.gameObject.SetActive(false);

			closeButton.onClick.AddListener(Close);
			Opened();
		}

		private void PackViewOnOnMouseClick(InventoryPackModel model)
		{
			_craftingController.StartCraft(_gameModel.PlayerModel.Inventory,
			                               _gameModel.PlayerModel.Inventory,
			                               model.Type);
		}

		private void PackViewOnOnMouseExit()
		{
			inventoryPopup.gameObject.SetActive(false);
			inventoryPopup.Reset();
		}

		private void PackViewOnOnMouseOver(InventoryPackModel model, CraftItemView obj)
		{
			inventoryPopup.SetData(_craftSettings.GetModel(model.Type));
			var rectTransform = obj.GetComponent<RectTransform>();
			var popupPosition = rectTransform.position;
			popupPosition.y                   -= rectTransform.sizeDelta.y / 4;
			inventoryPopup.transform.position =  popupPosition;
			inventoryPopup.gameObject.SetActive(true);
		}

		protected override void Closing()
		{
			_packViews.ForEach(view => view.Dispose());
			closeButton.onClick.RemoveAllListeners();
			Closed();
		}
	}
}