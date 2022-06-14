using System;
using UniRx;

namespace Helpers
{
	public static class RXHelper
	{
		public static IObservable<Tuple<TSource, TSource>> PairWithPrevious<TSource>(this IObservable<TSource> source)
		{
			return source.Scan(Tuple.Create(default(TSource), default(TSource)),
			                   (acc, current) => Tuple.Create(acc.Item2, current));
		}
	}
}