using System;
using System.Collections.Generic;
using System.Linq;
using Factories;
using ModestTree;
using UniRx;
using UnityEngine;

namespace Inventory
{
	public class FullInventoryPack : IDisposable
	{
		private IntReactiveProperty            _count = new IntReactiveProperty();
		public  IReadOnlyReactiveProperty<int> Count => _count;

		private List<InventoryPack>        _packs;
		private InventoryPack              _minPack;
		public  InventoryPackModel         Model { get; private set; }
		public  IEnumerable<InventoryPack> Packs => _packs;

		public void Initialize(IEnumerable<InventoryPack> packs, InventoryPackModel inventoryPackModel)
		{
			if (packs == null)
			{
				packs = new List<InventoryPack>();
			}

			_packs = packs.ToList();
			FindNextPack();
			Model = inventoryPackModel;
		}

		private void FindNextPack()
		{
			if (_packs.IsEmpty())
			{
				_minPack = null;
				_count.SetValueAndForceNotify(0);
			}
			else
			{
				_minPack = _packs.OrderBy(pack => pack.Size).First();
			}
		}

		public void Dispose()
		{
			foreach (var inventoryPack in _packs)
			{
				inventoryPack.Dispose();
			}

			Factory.ReturnItem(this);
		}

		public bool Add(int count = 1)
		{
			var totalCount = count;
			try
			{
				var ceil = Mathf.Ceil(count / (float)Model.MaxPackSize);
				if (ceil > 1)
				{
					for (var i = 0; i < ceil - 1; i++)
					{
						var pack = Factory.GetFactoryItem<InventoryPack>();
						pack.Initialize(Model, Model.MaxPackSize);
						_packs.Add(pack);
					}
				}

				count %= Model.MaxPackSize;
				AddSingle(count);
				_count.SetValueAndForceNotify(_count.Value += totalCount);
				return true;
			}
			catch (Exception)
			{
				return false;
			}
		}

		public bool Add(InventoryPack pack)
		{
			var totalCount = pack.Size;
			var packSize   = totalCount;
			pack.Dispose();
			Factory.ReturnItem(pack);
			try
			{
				AddSingle(packSize);
			}
			catch (Exception)
			{
				return false;
			}

			_count.SetValueAndForceNotify(_count.Value += totalCount);
			return true;
		}

		public bool Add(FullInventoryPack packs)
		{
			var totalCount = packs.Count.Value;
			var fullPacks  = packs._packs.Where(pack => pack.IsFull).ToArray();
			_packs.AddRange(fullPacks);
			packs._packs = packs._packs.Where(pack => fullPacks.All(inventoryPack => inventoryPack != pack)).ToList();
			var count = packs._packs.Sum(pack => pack.Size);
			packs.Dispose();
			_count.SetValueAndForceNotify(_count.Value += totalCount);
			return Add(count);
		}

		private void AddSingle(int count)
		{
			if (_minPack == null)
			{
				_minPack = Factory.GetFactoryItem<InventoryPack>();
				_minPack.Initialize(Model, count);
				_packs.Add(_minPack);
			}
			else
			{
				if (_minPack.Size + count > Model.MaxPackSize)
				{
					var pack = Factory.GetFactoryItem<InventoryPack>();
					pack.Initialize(Model, Model.MaxPackSize);
					_packs.Add(pack);
					_minPack.SetCount((_minPack.Size + count) % Model.MaxPackSize);
				}
				else
				{
					_minPack.AddItem(count);
				}
			}
		}

		public bool Remove(int count = 1)
		{
			var totalCount = count;
			if (_minPack?.Size > count)
			{
				return _minPack.RemoveItem(count);
			}

			if (_minPack?.Size == count)
			{
				return Remove(_minPack);
			}

			count -= _minPack?.Size ?? 0;
			_count.SetValueAndForceNotify(_count.Value -= totalCount);
			return Remove(_minPack) && _minPack.RemoveItem(count);
		}

		public bool Remove(InventoryPack pack)
		{
			try
			{
				var totalCount = pack.Size;
				_packs.Remove(pack);
				var needToFind = pack.Equals(_minPack);
				pack.Dispose();
				if (needToFind)
				{
					FindNextPack();
				}

				_count.SetValueAndForceNotify(_count.Value -= totalCount);
				return true;
			}
			catch (Exception)
			{
				return false;
			}
		}

		public void Clear()
		{
			_packs.ForEach(pack => pack.Dispose());
			_packs.Clear();
			_minPack = null;
		}
	}
}