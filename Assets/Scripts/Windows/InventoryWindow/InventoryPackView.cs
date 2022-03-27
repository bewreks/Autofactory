using System;
using Inventory;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Windows.InventoryWindow
{
	public class InventoryPackView : MonoBehaviour, IDisposable
	{
		public  Image           icon;
		public  TextMeshProUGUI count;
		private InventoryPack   _pack;

		public void SetData(InventoryPack pack)
		{
			_pack       = pack;
			icon.sprite = _pack.Icon;
			OnUpdateSize(_pack.Size);
			_pack.SizeChanged += OnUpdateSize;
			_pack.PackIsEmpty += OnEmpty;
		}

		private void OnEmpty()
		{
			Dispose();
			Destroy(gameObject);
		}

		public void OnUpdateSize(int size)
		{
			count.text = $"{size}/{_pack.MaxSize}";
		}

		public void Dispose()
		{
			_pack.SizeChanged -= OnUpdateSize;
		}
	}
}