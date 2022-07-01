using System;
using System.Collections.Generic;
using Crafting;
using Inventories;
using UnityEngine;
using UnityEngine.UI;

namespace Windows.CraftingWindow
{
	public class CraftingWindowView : WindowView
	{
		[SerializeField] private CraftItemView  craftItemPrefab;
		[SerializeField] private Button         closeButton;
		[SerializeField] private Transform      craftContent;
		[SerializeField] private InventoryPopup inventoryPopup;

		private List<CraftItemView> _packViews = new List<CraftItemView>();
		public  CraftItemView       CraftPackPrefab => craftItemPrefab;

		public event Action<InventoryPackModel, RectTransform> OnMouseOver;
		public event Action                                    OnMouseClick;
		public event Action                                    OnMouseExit;

		public override void Opening(float duration) { }

		public void ShowModels(List<CraftingModel> models, CraftItemView prefab)
		{
			foreach (var craftingModel in models)
			{
				var packView = Instantiate(prefab,
				                           Vector3.zero,
				                           Quaternion.identity,
				                           craftContent).GetComponent<CraftItemView>();
				packView.SetData(craftingModel.CraftingResult);
				packView.OnMouseOver  += PackViewOnOnMouseOver;
				packView.OnMouseExit  += PackViewOnOnMouseExit;
				packView.OnMouseClick += PackViewOnOnMouseClick;
				_packViews.Add(packView);
			}
		}

		public override void AfterOpen()
		{
			closeButton.onClick.AddListener(CastOnClose);
		}

		private void PackViewOnOnMouseClick(InventoryPackModel model)
		{
			OnMouseClick?.Invoke();
		}

		private void PackViewOnOnMouseExit()
		{
			OnMouseExit?.Invoke();
		}

		private void PackViewOnOnMouseOver(InventoryPackModel model, CraftItemView obj)
		{
			OnMouseOver?.Invoke(model, obj.GetComponent<RectTransform>());
		}

		public override void Closing(float duration)
		{
			_packViews.ForEach(view => view.Dispose());
			closeButton.onClick.RemoveAllListeners();
		}

		public override void Hiding(float duration)
		{
			_packViews.ForEach(view => view.Dispose());
			closeButton.onClick.RemoveAllListeners();
		}

		public void ShowHint(CraftingModel model, RectTransform rectTransform)
		{
			inventoryPopup.SetData(model);
			var popupPosition = rectTransform.position;
			popupPosition.y                   -= rectTransform.sizeDelta.y / 4;
			inventoryPopup.transform.position =  popupPosition;
			inventoryPopup.gameObject.SetActive(true);
		}

		public void HideHint()
		{
			inventoryPopup.gameObject.SetActive(false);
			inventoryPopup.Reset();
		}
	}
}