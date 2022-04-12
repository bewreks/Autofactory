using System;
using UniRx;
using UnityEngine;

namespace Inventory
{
	[CreateAssetMenu(fileName = "NewPackModel", menuName = "Models/Inventory/InventoryPack", order = 0)]
	public class InventoryPackModel : ScriptableObject
	{
		[SerializeField] private InventoryTypesEnum type        = InventoryTypesEnum.NOTHING;
		[SerializeField] private int                maxPackSize = 100;
		[SerializeField] private Sprite             icon;
		[SerializeField] private GameObject         instance;

		public InventoryTypesEnum Type        => type;
		public int                MaxPackSize => maxPackSize;
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
		public event Action PackIsFull;
		public event Action PackIsEmpty;

		private IntReactiveProperty _size = new IntReactiveProperty();

		public IReadOnlyReactiveProperty<int> Size    => _size;
		public Sprite                         Icon    => Model.Icon;
		public bool                           IsFull  => _size.Value >= Model.MaxPackSize;
		public bool                           IsEmpty => _size.Value <= 0;
		public InventoryPackModel             Model   { get; private set; }

		public void Initialize(InventoryPackModel model, int size = 1)
		{
			Model = model;
			_size.SetValueAndForceNotify(size);
		}

		private bool UpdateSize(int newSize, Predicate<int> check, Action @event)
		{
			if (check.Invoke(_size.Value)) return false;

			_size.SetValueAndForceNotify(newSize);
			if (check.Invoke(_size.Value))
			{
				@event?.Invoke();
			}

			return true;
		}

		public void Dispose()
		{
			PackIsEmpty = null;
			PackIsFull  = null;
			Model       = null;
			_size.Value = 0;
		}

		public bool AddItem(int count = 1)
		{
			return UpdateSize(_size.Value + count, size => size >= Model.MaxPackSize, PackIsFull);
		}

		public bool SetCount(int count)
		{
			return UpdateSize(count, size => size >= Model.MaxPackSize, PackIsFull);
		}

		public bool RemoveItem(int count = 1)
		{
			return UpdateSize(_size.Value - count, size => size <= 0, PackIsEmpty);
		}
	}

	public enum InventoryTypesEnum
	{
		NOTHING,
		TEST_OBJECT,
		GENERATOR
	}
}