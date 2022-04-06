using System;
using System.Collections.Generic;
using ModestTree;
using Zenject;

namespace Factories
{
	public static class Factory
	{
		private static Dictionary<Type, Queue<object>> _objects = new Dictionary<Type, Queue<object>>();

		public static T GetFactoryItem<T>() where T : new()
		{
			return GetFactoryItem(() =>
			{
				var item = new T();
				return item;
			});
		}
		
		public static T GetFactoryItem<T>(DiContainer _container) where T : new()
		{
			return GetFactoryItem(() =>
			{
				var item = new T();
				_container.Inject(item);
				return item;
			});
		}

		private static T GetFactoryItem<T>(Func<T> callback) where T : new()
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
				item = callback();
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