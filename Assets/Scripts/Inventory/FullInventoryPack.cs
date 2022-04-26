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
			Model = inventoryPackModel;
			if (Model == null)
			{
				throw new NullReferenceException("Model is null");
			}

			if (packs == null)
			{
				packs = new List<InventoryPack>();
			}

			_packs = new List<InventoryPack>();
			foreach (var pack in packs)
			{
				if (_minPack == null)
				{
					_minPack = pack;
					_packs.Add(_minPack);
					continue;
				}

				var edge = _minPack.AddItem(pack.Size.Value);
				if (edge != 0)
				{
					_minPack = Factory.GetFactoryItem<InventoryPack>();
					_minPack.Initialize(Model, edge);
					_packs.Add(_minPack);
				}
			}

			_count.SetValueAndForceNotify(_packs.Sum(pack => pack.Size.Value));
			FindNextPack();
		}

		private void FindNextPack()
		{
			_minPack = _packs.IsEmpty() ? null : _packs.OrderBy(pack => pack.Size.Value).First();
		}

		public void Dispose()
		{
			Clear();

			Factory.ReturnItem(this);
		}

		public int Add(int count = 1)
		{
			if (count < 0)
			{
				count = 0;
			}

			if (count == 0) return 0;

			var totalCount = count;
			try
			{
				count = AddFulls(count);
				AddSingle(count);
				_count.SetValueAndForceNotify(_count.Value += totalCount);
				return 0;
			}
			catch (Exception)
			{
				return 0;
			}
		}

		private int AddFulls(int count)
		{
			var ceil = Mathf.Floor(count / (float)Model.MaxPackSize);
			if (ceil > 0)
			{
				for (var i = 0; i < ceil; i++)
				{
					var pack = Factory.GetFactoryItem<InventoryPack>();
					pack.Initialize(Model, Model.MaxPackSize);
					_packs.Add(pack);
				}
			}
			
			FindNextPack();

			count %= Model.MaxPackSize;
			return count;
		}

		public int Add(InventoryPack pack)
		{
			var totalCount = pack.Size.Value;
			try
			{
				AddSingle(totalCount);
			}
			catch (Exception)
			{
				return 0;
			}
			finally
			{
				pack.Dispose();
				Factory.ReturnItem(pack);
			}

			_count.SetValueAndForceNotify(_count.Value += totalCount);
			return 0;
		}

		public int Add(FullInventoryPack packs)
		{
			if (packs.Equals(this)) return 0;

			var totalCount = packs.Count.Value;
			var fullPacks  = packs._packs.Where(pack => pack.IsFull).ToArray();
			_packs.AddRange(fullPacks);
			packs._packs = packs._packs.Where(pack => fullPacks.All(inventoryPack => inventoryPack != pack)).ToList();
			var count = packs._packs.Sum(pack => pack.Size.Value);
			packs.Dispose();
			AddSingle(AddFulls(count));
			_count.SetValueAndForceNotify(_count.Value += totalCount);
			return 0;
		}

		private void AddSingle(int count)
		{
			if (count == 0) return;
			if (_minPack == null)
			{
				_minPack = Factory.GetFactoryItem<InventoryPack>();
				_minPack.Initialize(Model, count);
				_packs.Add(_minPack);
			}
			else
			{
				if (_minPack.Size.Value + count > Model.MaxPackSize)
				{
					var pack = Factory.GetFactoryItem<InventoryPack>();
					pack.Initialize(Model, Model.MaxPackSize);
					_packs.Add(pack);
					_minPack.SetCount((_minPack.Size.Value + count) % Model.MaxPackSize);
				}
				else
				{
					_minPack.AddItem(count);
				}
			}
		}

		public int Remove(int count = 1)
		{
			if (count < 0)
			{
				count = 0;
			}

			if (count == 0) return 0;

			var edge = count;

			if (edge > Model.MaxPackSize)
			{
				var maxPacks       = edge % Model.MaxPackSize;
				var inventoryPacks = _packs.Where(pack => pack.IsFull).Take(maxPacks).ToList();
				inventoryPacks.ForEach(pack =>
				{
					if (_minPack == pack)
					{
						_minPack = null;
					}

					_packs.Remove(pack);
				});
				if (_minPack == null)
				{
					FindNextPack();
				}

				edge -= Model.MaxPackSize * (inventoryPacks.Count % Model.MaxPackSize);
			}

			if (_minPack != null)
			{
				while (edge > 0)
				{
					edge = _minPack.RemoveItem(edge);
					if (_minPack.IsEmpty)
					{
						RemoveSpecificPack(_minPack);
					}

					if (_minPack == null)
					{
						break;
					}
				}
			}

			_count.SetValueAndForceNotify(_count.Value - (count - edge));
			return edge;
		}

		public int Remove(InventoryPack pack)
		{
			try
			{
				if (_packs.Contains(pack))
				{
					var totalCount = RemoveSpecificPack(pack);

					_count.SetValueAndForceNotify(_count.Value -= totalCount);
				}

				return 0;
			}
			catch (Exception)
			{
				return 0;
			}
		}

		private int RemoveSpecificPack(InventoryPack pack)
		{
			if (_packs.Remove(pack))
			{
				var size       = pack.Size.Value;
				var needToFind = pack.Equals(_minPack);
				pack.Dispose();
				if (needToFind)
				{
					FindNextPack();
				}

				return size;
			}
			else
			{
				return 0;
			}
		}

		public void Clear()
		{
			_packs.ForEach(pack => pack.Dispose());
			_packs.Clear();
			_count.Value = 0;
			_minPack     = null;
		}
	}
}