using System;
using UnityEngine;

namespace Inventory
{
	[CreateAssetMenu(fileName = "NewPackModel", menuName = "Models/Inventory/InventoryPack", order = 0)]
	public class InventoryPackModel : ScriptableObject
	{
		[SerializeField] private InventoryTypesEnum type        = InventoryTypesEnum.NOTHING;
		[SerializeField] private uint               maxPackSize = 100;

		public InventoryTypesEnum Type        => type;
		public uint               MaxPackSize => maxPackSize;
	}

	public class InventoryPack
	{
		public event Action<int> SizeChanged;
		public event Action      PackIsFull;
		public event Action      PackIsEmpty;

		private int                _size;
		private InventoryPackModel _model;

		public int                Size    => _size;
		public bool               IsFull  => _size >= _model.MaxPackSize;
		public bool               IsEmpty => _size <= 0;
		public InventoryTypesEnum Type    => _model.Type;

		public void Initialize(InventoryPackModel model, int size = 1)
		{
			_model = model;
			_size  = size;
		}

		public bool AddItem()
		{
			return UpdateSize(_size + 1, size => size >= _model.MaxPackSize, PackIsFull);
		}

		public bool RemoveItem()
		{
			return UpdateSize(_size - 1, size => size <= 0, PackIsEmpty);
		}

		private bool UpdateSize(int newSize, Predicate<int> check, Action @event)
		{
			if (check.Invoke(_size)) return false;

			_size = newSize;
			SizeChanged?.Invoke(_size);
			if (check.Invoke(_size))
			{
				@event?.Invoke();
			}

			return true;
		}

		public void Reset()
		{
			SizeChanged = null;
			PackIsEmpty = null;
			PackIsFull  = null;
			_model      = null;
			_size       = 0;
		}
	}

	public enum InventoryTypesEnum
	{
		NOTHING,
		GENERATOR
	}
}