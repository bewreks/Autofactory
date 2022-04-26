using System;
using System.Collections.Generic;
using ModestTree;
using Zenject;

namespace Factories
{
	public static class Factory
	{
		private static Dictionary<Type, Queue<object>> _objects    = new Dictionary<Type, Queue<object>>();
		private static Dictionary<Type, int>           _objectsIDs = new Dictionary<Type, int>();

		public static T GetFactoryItemWithID<T>() where T : IFactoryItem, new()
		{
			return GetFactoryItem(() =>
			{
				var item = new T();
				item.ID = _objectsIDs[typeof(T)];
				return item;
			});
		}

		public static T GetFactoryItem<T>() where T : new()
		{
			return GetFactoryItem(() =>
			{
				var item = new T();
				return item;
			});
		}

		public static T GetFactoryItem<T>(DiContainer container) where T : new()
		{
			return GetFactoryItem(() =>
			{
				var item = new T();
				container.Inject(item);
				return item;
			});
		}

		private static T GetFactoryItem<T>(Func<T> callback)
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
				_objectsIDs.Add(typeof(T), Int32.MinValue);
			}

			if (EqualityComparer<T>.Default.Equals(item, default))
			{
				item = callback();
			}


			return item;
		}

		public static void ReturnItem<T>(T obj)
		{
			if (!_objects.TryGetValue(typeof(T), out var objects))
			{
				objects = new Queue<object>();
				_objects.Add(typeof(T), objects);
			}

			objects.Enqueue(obj);
		}

		public static int GetCountOf<T>()
		{
			if (!_objects.TryGetValue(typeof(T), out var objects))
			{
				return 0;
			}

			return objects.Count;
		}

		public static void Clear()
		{
			_objects.Clear();
			_objectsIDs.Clear();
		}
	}

	public interface IFactoryItem
	{
		int ID { get; set; }
	}
}