using System;
using System.Collections.Generic;
using System.Linq;
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

			buildings.AddUnique(data);
		}

		public static void AddUniqueRange<K, T>(this Dictionary<K, List<T>> dictionary,
		                                        K                           key,
		                                        List<T>                     data)
		{
			if (!dictionary.TryGetValue(key, out var buildings))
			{
				buildings = new List<T>();
				dictionary.Add(key, buildings);
			}

			buildings.AddUniqueRange(data);
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
			list.AddUniqueRange(dataEnum, out var notUsed);
		}

		public static void AddUniqueRange<T>(this List<T> list, IEnumerable<T> dataEnum, out IEnumerable<T> added)
		{
			added = dataEnum.Except(list);
			list.AddRange(added);
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