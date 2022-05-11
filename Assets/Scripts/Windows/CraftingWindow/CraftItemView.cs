using System;
using Crafting;
using Inventories;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Windows.CraftingWindow
{
	public class CraftItemView : MonoBehaviour, IDisposable, IPointerEnterHandler, IPointerExitHandler,
	                                 IPointerClickHandler
	{
		public  Image              icon;
		public  TextMeshProUGUI    count;
		private InventoryPack      _pack;
		private InventoryPackModel _model;
		private IDisposable        _sizeSub;

		public event Action<InventoryPackModel, CraftItemView> OnMouseOver;
		public event Action<InventoryPackModel>                OnMouseClick;
		public event Action                                    OnMouseExit;

		public void SetData(CraftingNeed craftingModelResult)
		{
			transform.localScale = Vector3.one;
			_model               = craftingModelResult.model;
			icon.sprite          = _model.Icon;
			count.text           = craftingModelResult.count.ToString();
		}

		private void OnEmpty()
		{
			Dispose();
			Destroy(gameObject);
		}

		public void OnUpdateSize(int size)
		{
			if (!_pack.Disposed)
				count.text = $"{size}/{_pack.Model.MaxPackSize}";
		}

		public void Dispose()
		{
			_sizeSub?.Dispose();
			_sizeSub     = null;
			OnMouseOver  = null;
			OnMouseExit  = null;
			OnMouseClick = null;
		}

		public void OnPointerEnter(PointerEventData eventData)
		{
			OnMouseOver?.Invoke(_model, this);
		}

		public void OnPointerExit(PointerEventData eventData)
		{
			OnMouseExit?.Invoke();
		}

		public void OnPointerClick(PointerEventData eventData)
		{
			OnMouseClick?.Invoke(_model);
		}
	}
}