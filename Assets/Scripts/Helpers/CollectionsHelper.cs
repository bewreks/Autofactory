using System;
using System.Collections.Generic;
using JetBrains.Annotations;

namespace Helpers
{
	public static class CollectionsHelper
	{
		public static void AddUnique<K, T>(this Dictionary<K, List<T>> dictionary,
		                                   K                           key,
		                                   T                           data)
		{
			if (!dictionary.TryGetValue(key, out var buildings))
			{
				buildings = new List<T>();
				dictionary.Add(key, buildings);
			}

			if (!buildings.Contains(data))
			{
				buildings.AddUnique(data);
			}
		}

		public static void AddUnique<T>(this List<T> list, T data)
		{
			if (!list.Contains(data))
			{
				list.Add(data);
			}
		}

		public static void AddUniqueRange<T>(this List<T> list, IEnumerable<T> dataEnum)
		{
			foreach (var data in dataEnum)
			{
				list.AddUnique(data);
			}
		}

		public static void ForEveryKey<T, K>(this Dictionary<K, T> map, [NotNull] Action<K, T> @delegate)
		{
			if (@delegate == null) throw new ArgumentNullException(nameof(@delegate));
			foreach (var key in map.Keys)
			{
				@delegate.Invoke(key, map[key]);
			}
		}
	}
}