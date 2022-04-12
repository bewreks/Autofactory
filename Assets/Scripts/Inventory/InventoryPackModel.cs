using System;
using UnityEngine;

namespace Inventory
{
	[CreateAssetMenu(fileName = "NewPackModel", menuName = "Models/Inventory/InventoryPack", order = 0)]
	public class InventoryPackModel : ScriptableObject
	{
		[SerializeField] private InventoryTypesEnum type        = InventoryTypesEnum.NOTHING;
		[SerializeField] private int               maxPackSize = 100;
		[SerializeField] private Sprite             icon;
		[SerializeField] private GameObject         instance;

		public InventoryTypesEnum Type        => type;
		public int               MaxPackSize => maxPackSize;
		public Sprite             Icon        => icon;
		public GameObject         Instance    => instance;

#if UNITY_INCLUDE_TESTS
		public static InventoryPackModel GetTestModel()
		{
			var inventoryPackModel = CreateInstance<InventoryPackModel>();
			inventoryPackModel.type        = InventoryTypesEnum.TEST_OBJECT;
			inventoryPackModel.maxPackSize = 50;
			inventoryPackModel.icon        = SpriteHelper.GetBlankSprite(128, 128);
			inventoryPackModel.instance    = new GameObject();
			return inventoryPackModel;
		}
#endif
	}

	public class InventoryPack : IDisposable
	{
		public event Action<int> SizeChanged;
		public event Action      PackIsFull;
		public event Action      PackIsEmpty;

		private int _size;

		public int                Size    => _size;
		public Sprite             Icon    => Model.Icon;
		public bool               IsFull  => _size >= Model.MaxPackSize;
		public bool               IsEmpty => _size <= 0;
		public InventoryPackModel Model   { get; private set; }

		public void Initialize(InventoryPackModel model, int size = 1)
		{
			Model = model;
			_size = size;
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

		public void Dispose()
		{
			SizeChanged = null;
			PackIsEmpty = null;
			PackIsFull  = null;
			Model       = null;
			_size       = 0;
		}

		public bool AddItem(int count = 1)
		{
			return UpdateSize(_size + count, size => size >= Model.MaxPackSize, PackIsFull);
		}

		public bool SetCount(int count)
		{
			return UpdateSize(count, size => size >= Model.MaxPackSize, PackIsFull);
		}

		public bool RemoveItem(int count = 1)
		{
			return UpdateSize(_size - count, size => size <= 0, PackIsEmpty);
		}
	}

	public enum InventoryTypesEnum
	{
		NOTHING,
		TEST_OBJECT,
		GENERATOR
	}
}