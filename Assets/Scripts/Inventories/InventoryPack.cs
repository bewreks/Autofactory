using System;
using Factories;
using UniRx;
using UnityEngine;

namespace Inventories
{
	public class InventoryPack : IDisposable
	{
		public event Action PackIsFull;
		public event Action PackIsEmpty;

		private IntReactiveProperty _size = new IntReactiveProperty();

		public IReadOnlyReactiveProperty<int> Size     => _size;
		public Sprite                         Icon     => Model.Icon;
		public bool                           IsFull   => _size.Value >= Model.MaxPackSize;
		public bool                           IsEmpty  => _size.Value <= 0;
		public InventoryPackModel             Model    { get; private set; }
		public bool                           Disposed => Model == null;

		public int Initialize(InventoryPackModel model, int size = 1)
		{
			Model = model;
			if (size > model.MaxPackSize)
			{
				SetSize(model.MaxPackSize);
				return size - model.MaxPackSize;
			}

			SetSize(size <= 0 ? 0 : size);

			return 0;
		}

		private int UpdateSize(int newSize, Predicate<int> check, Action @event)
		{
			var edge = 0;

			if (newSize > Model.MaxPackSize)
			{
				edge = newSize - Model.MaxPackSize;
				SetSize(Model.MaxPackSize);
			}
			else
			{
				SetSize(newSize);
			}

			if (check.Invoke(_size.Value))
			{
				@event?.Invoke();
			}

			return edge;
		}

		private void SetSize(int newSize)
		{
			if (_size.Value != newSize)
			{
				_size.SetValueAndForceNotify(newSize);
			}
		}

		public void Dispose()
		{
			PackIsEmpty = null;
			PackIsFull  = null;
			Model       = null;
			_size.Value = 0;
			Factory.ReturnItem(this);
		}

		public int AddItem(int count = 1)
		{
			if (count < 0)
			{
				count = 0;
			}

			if (count == 0) return 0;

			var edge    = 0;
			var newSize = _size.Value + count;
			if (newSize > Model.MaxPackSize)
			{
				edge = newSize - Model.MaxPackSize;
				SetSize(Model.MaxPackSize);
			}
			else
			{
				SetSize(newSize);
			}

			if (_size.Value >= Model.MaxPackSize)
			{
				PackIsFull?.Invoke();
			}

			return edge;
		}

		public int SetCount(int count)
		{
			if (count < 0)
			{
				count = 0;
			}

			var edge    = 0;
			var newSize = count;
			if (newSize > Model.MaxPackSize)
			{
				edge = newSize - Model.MaxPackSize;
				SetSize(Model.MaxPackSize);
			}
			else
			{
				SetSize(newSize);
			}

			if (_size.Value >= Model.MaxPackSize)
			{
				PackIsFull?.Invoke();
			}

			if (_size.Value <= 0)
			{
				PackIsEmpty?.Invoke();
			}

			return edge;
		}

		public int RemoveItem(int count = 1)
		{
			if (count < 0)
			{
				count = 0;
			}

			var edge    = 0;
			var newSize = _size.Value - count;
			if (newSize < 0)
			{
				edge = Mathf.Abs(newSize);
				SetSize(0);
			}
			else
			{
				SetSize(newSize);
			}

			if (_size.Value <= 0)
			{
				PackIsEmpty?.Invoke();
			}

			return edge;
		}
	}
}