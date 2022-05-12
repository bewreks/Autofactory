using System;
using Crafting;
using Inventories;
using TMPro;
using UniRx;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Windows.InventoryWindow
{
	public class InventoryPackView : MonoBehaviour, IDisposable
	{
		public  Image              icon;
		public  TextMeshProUGUI    count;
		private InventoryPack      _pack;
		private InventoryPackModel _model;
		private IDisposable        _sizeSub;

		public void SetData(InventoryPack pack)
		{
			transform.localScale =  Vector3.one;
			_pack                =  pack;
			_model               =  _pack.Model;
			icon.sprite          =  _pack.Icon;
			_sizeSub             =  _pack.Size.Subscribe(OnUpdateSize);
			_pack.PackIsEmpty    += OnEmpty;
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
			_sizeSub          =  null;
			_pack.PackIsEmpty -= OnEmpty;
		}
	}
}