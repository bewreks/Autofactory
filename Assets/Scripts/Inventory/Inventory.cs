﻿using System;
using System.Collections.Generic;
using System.Linq;
using ModestTree;
using Zenject;

namespace Inventory
{
	public class Inventory : IInventory
	{
		[Inject] private InventoryPacksModelsManager _inventoryPacksModelsManager;
		
		private List<InventoryPack> _packs;

		public Inventory()
		{
			_packs = new List<InventoryPack>();
		}

		public bool AddItem(InventoryTypesEnum type)
		{
			var pack = _packs.FirstOrDefault(pack => pack.Type == type && !pack.IsFull);
			if (pack == null)
			{
				try
				{
					pack = InventoryFactory.GetFactoryItem<InventoryPack>();
					pack.Initialize(_inventoryPacksModelsManager.GetModel(type));
					_packs.Add(pack);
				}
				catch (Exception)
				{
					return false;
				}
			}
			else
			{
				return pack.AddItem();
			}

			return true;
		}

		public bool RemoveItem(InventoryTypesEnum type)
		{
			var pack = _packs.FirstOrDefault(pack => pack.Type == type && !pack.IsEmpty);

			if (pack == null) return false;

			var removeItemResult = pack.RemoveItem();

			if (pack.IsEmpty)
			{
				pack.Reset();
				_packs.Remove(pack);
				InventoryFactory.ReturnItem(pack);
			}
			
			return removeItemResult;
		}

		public void Dispose()
		{
			_packs.ForEach(pack => pack.Reset());
		}

		public List<InventoryPack> GetItems()
		{
			return _packs;
		}

		public int GetItemsCount(InventoryTypesEnum type)
		{
			var packs = _packs.Where(pack => pack.Type == type && !pack.IsFull);
			return packs.Sum(inventoryPack => inventoryPack.Size);
		}
	}

	public static class InventoryFactory
	{
		private static Dictionary<Type, Queue<object>> _objects = new Dictionary<Type, Queue<object>>();

		public static T GetFactoryItem<T>() where T : new()
		{
			var item = default(T);

			if (_objects.TryGetValue(typeof(T), out var objects))
			{
				if (!objects.IsEmpty())
				{
					item = (T)objects.Dequeue();
				}
			}
			else
			{
				_objects.Add(typeof(T), new Queue<object>());
			}

			if (EqualityComparer<T>.Default.Equals(item, default))
			{
				item = new T();
			}

			return item;
		}

		public static void ReturnItem<T>(T obj)
		{
			Queue<object> objects;
			if (!_objects.TryGetValue(typeof(T), out objects))
			{
				objects = new Queue<object>();
				_objects.Add(typeof(T), objects);
			}
			
			objects.Enqueue(obj);
		}
#if UNITY_INCLUDE_TESTS
		public static void Clear()
		{
			_objects.Clear();
		}

		public static Dictionary<Type, Queue<object>> Objects => _objects;
	}
#endif
}