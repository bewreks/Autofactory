using System.Collections.Generic;
using System.Linq;
using ModestTree;

namespace Factories
{
	public class IDFactory
	{
		private int _curID;

		private List<int> _returned = new List<int>();

		public int Pop()
		{
			if (!_returned.IsEmpty())
			{
				var min = _returned.Min();
				_returned.Remove(min);
				return min;
			}
			return _curID++;
		}

		public void Push(int id)
		{
			_returned.Add(id);
		}
	}
}