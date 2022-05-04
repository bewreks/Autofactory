using System;
using Inventories;
using TMPro;
using UniRx;
using UnityEngine;
using UnityEngine.UI;

namespace Windows.InventoryWindow
{
	public class InventoryPackView : MonoBehaviour, IDisposable
	{
		public  Image           icon;
		public  TextMeshProUGUI count;
		private InventoryPack   _pack;
		private IDisposable     _sizeSub;

		public void SetData(InventoryPack pack)
		{
			_pack             =  pack;
			icon.sprite       =  _pack.Icon;
			_sizeSub          =  _pack.Size.Subscribe(OnUpdateSize);
			_pack.PackIsEmpty += OnEmpty;
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
			_sizeSub = null;
		}
	}
}