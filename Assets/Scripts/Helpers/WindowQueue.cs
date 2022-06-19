using System;
using System.Collections;
using System.Collections.Generic;
using Windows;

namespace Helpers
{
	// Deque but not deque
	// По сути нужен двусвязный список с уникальными значениями
	// Но с хэш картой
	public class WindowQueue : IEnumerable<IWindow>
	{
		private          DoublyNode<IWindow>                   _head;
		private          DoublyNode<IWindow>                   _tail;
		private          int                                   _count;
		private readonly Dictionary<Type, DoublyNode<IWindow>> _listMap;

		public WindowQueue()
		{
			_listMap = new Dictionary<Type, DoublyNode<IWindow>>();
		}

		public void AddLast(IWindow window)
		{
			var type = window.WindowType;
			if (_listMap.TryGetValue(type, out var node))
			{
				CutNode(node);
			}
			else
			{
				node = new DoublyNode<IWindow>(window);
				_listMap.Add(type, node);
				_count++;
			}

			if (_head == null)
				_head = node;
			else
			{
				_tail.Next    = node;
				node.Previous = _tail;
			}

			_tail = node;
		}

		public void AddFirst(IWindow window)
		{
			var type = window.WindowType;
			if (_listMap.TryGetValue(type, out var node))
			{
				CutNode(node);
			}
			else
			{
				node = new DoublyNode<IWindow>(window);
				_listMap.Add(type, node);
				_count++;
			}

			if (_tail == null)
			{
				_tail = node;
			}
			else
			{
				_head.Previous = node;
			}
			node.Next = _head;
			_head     = node;
		}

		public IWindow RemoveFirst()
		{
			if (_count == 0)
			{
				return null;
			}
			
			var output = _head.Data;
			if (_count == 1)
			{
				_head = _tail = null;
			}
			else
			{
				_head          = _head.Next;
				_head.Previous = null;
			}

			_count--;
			_listMap.Remove(output.WindowType);
			return output;
		}

		public IWindow First
		{
			get
			{
				if (IsEmpty)
					throw new InvalidOperationException();
				return _head.Data;
			}
		}

		public int Count => _count;

		public bool IsEmpty => _count == 0;

		public void Clear()
		{
			_head  = null;
			_tail  = null;
			_count = 0;
			_listMap.Clear();
		}

		public bool Contains(IWindow window)
		{
			return _listMap.ContainsKey(window.WindowType);
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			return ((IEnumerable)this).GetEnumerator();
		}

		IEnumerator<IWindow> IEnumerable<IWindow>.GetEnumerator()
		{
			var current = _head;
			while (current != null)
			{
				yield return current.Data;
				current = current.Next;
			}
		}

		private void CutNode(DoublyNode<IWindow> node)
		{
			if (node.Previous != null)
			{
				node.Previous.Next = node.Next;
			}
			else
			{
				_head = node.Next;
			}
			if (node.Next != null)
			{
				node.Next.Previous = node.Previous;
			}
			else
			{
				_tail = node.Previous;
			}

			node.Next     = null;
			node.Previous = null;
		}

#if UNITY_INCLUDE_TESTS
		public DoublyNode<IWindow>                   Head    => _head;
		public DoublyNode<IWindow>                   Tail    => _tail;
		public Dictionary<Type, DoublyNode<IWindow>> ListMap => _listMap;
#endif
	}
}