using Windows;
using Helpers;
using JetBrains.Annotations;
using Moq;
using NUnit.Framework;

namespace Tests.Helpers
{
	[TestFixture]
	public class WindowQueueTest
	{
		private WindowQueue _windowQueue;

		[SetUp]
		public void Setup()
		{
			_windowQueue = new WindowQueue();
		}

		[TearDown]
		public void TearDown()
		{
			_windowQueue.Clear();
			_windowQueue = null;
		}

		[Test]
		public void AddFirstOneTest()
		{
			var windowMock = CreateWindowMock<TestWindowTypeOne>();
			_windowQueue.AddFirst(windowMock.Object);
			Assert.IsFalse(_windowQueue.IsEmpty);
			Assert.IsTrue(_windowQueue.Contains(windowMock.Object));
			Assert.AreEqual(_windowQueue.Head.Data, windowMock.Object);
			Assert.AreEqual(_windowQueue.Tail.Data, windowMock.Object);
			Assert.AreEqual(1,                      _windowQueue.Count);
			Assert.AreEqual(1,                      _windowQueue.ListMap.Count);
			TestNode(_windowQueue.Head, new[] { windowMock.Object });
		}

		[Test]
		public void AddFirstTwoTest()
		{
			var firstWindowMock  = CreateWindowMock<TestWindowTypeOne>();
			var secondWindowMock = CreateWindowMock<TestWindowTypeTwo>();
			_windowQueue.AddFirst(firstWindowMock.Object);
			_windowQueue.AddFirst(secondWindowMock.Object);
			Assert.IsFalse(_windowQueue.IsEmpty);
			Assert.IsTrue(_windowQueue.Contains(firstWindowMock.Object));
			Assert.IsTrue(_windowQueue.Contains(secondWindowMock.Object));
			Assert.AreEqual(_windowQueue.Head.Data, secondWindowMock.Object);
			Assert.AreEqual(_windowQueue.Tail.Data, firstWindowMock.Object);
			Assert.AreEqual(2,                      _windowQueue.Count);
			Assert.AreEqual(2,                      _windowQueue.ListMap.Count);
			TestNode(_windowQueue.Head, new[]
			                            {
				                            secondWindowMock.Object,
				                            firstWindowMock.Object,
			                            });
		}

		[Test]
		public void AddFirstTwoWithDoubleFirstTest()
		{
			var firstWindowMock  = CreateWindowMock<TestWindowTypeOne>();
			var secondWindowMock = CreateWindowMock<TestWindowTypeTwo>();
			_windowQueue.AddFirst(firstWindowMock.Object);
			_windowQueue.AddFirst(secondWindowMock.Object);
			_windowQueue.AddFirst(firstWindowMock.Object);
			Assert.IsFalse(_windowQueue.IsEmpty);
			Assert.IsTrue(_windowQueue.Contains(firstWindowMock.Object));
			Assert.IsTrue(_windowQueue.Contains(secondWindowMock.Object));
			Assert.AreEqual(_windowQueue.Head.Data, firstWindowMock.Object);
			Assert.AreEqual(_windowQueue.Tail.Data, secondWindowMock.Object);
			Assert.AreEqual(2,                      _windowQueue.Count);
			Assert.AreEqual(2,                      _windowQueue.ListMap.Count);
			TestNode(_windowQueue.Head, new[]
			                            {
				                            firstWindowMock.Object,
				                            secondWindowMock.Object,
			                            });
		}

		[Test]
		public void AddFirstFourWithDoubleFirstTest()
		{
			var firstWindowMock  = CreateWindowMock<TestWindowTypeOne>();
			var secondWindowMock = CreateWindowMock<TestWindowTypeTwo>();
			var thirdWindowMock  = CreateWindowMock<TestWindowTypeThree>();
			var fourWindowMock   = CreateWindowMock<TestWindowTypeFour>();
			_windowQueue.AddFirst(firstWindowMock.Object);
			_windowQueue.AddFirst(secondWindowMock.Object);
			_windowQueue.AddFirst(thirdWindowMock.Object);
			_windowQueue.AddFirst(fourWindowMock.Object);
			_windowQueue.AddFirst(firstWindowMock.Object);
			Assert.IsFalse(_windowQueue.IsEmpty);
			Assert.IsTrue(_windowQueue.Contains(firstWindowMock.Object));
			Assert.IsTrue(_windowQueue.Contains(secondWindowMock.Object));
			Assert.IsTrue(_windowQueue.Contains(thirdWindowMock.Object));
			Assert.IsTrue(_windowQueue.Contains(fourWindowMock.Object));
			Assert.AreEqual(_windowQueue.Head.Data, firstWindowMock.Object);
			Assert.AreEqual(_windowQueue.Tail.Data, secondWindowMock.Object);
			Assert.AreEqual(4,                      _windowQueue.Count);
			Assert.AreEqual(4,                      _windowQueue.ListMap.Count);
			TestNode(_windowQueue.Head, new[]
			                            {
				                            firstWindowMock.Object,
				                            fourWindowMock.Object,
				                            thirdWindowMock.Object,
				                            secondWindowMock.Object,
			                            });
		}

		[Test]
		public void AddFirstFourWithDoubleSecondTest()
		{
			var firstWindowMock  = CreateWindowMock<TestWindowTypeOne>();
			var secondWindowMock = CreateWindowMock<TestWindowTypeTwo>();
			var thirdWindowMock  = CreateWindowMock<TestWindowTypeThree>();
			var fourWindowMock   = CreateWindowMock<TestWindowTypeFour>();
			_windowQueue.AddFirst(firstWindowMock.Object);
			_windowQueue.AddFirst(secondWindowMock.Object);
			_windowQueue.AddFirst(thirdWindowMock.Object);
			_windowQueue.AddFirst(fourWindowMock.Object);
			_windowQueue.AddFirst(secondWindowMock.Object);
			Assert.IsFalse(_windowQueue.IsEmpty);
			Assert.IsTrue(_windowQueue.Contains(firstWindowMock.Object));
			Assert.IsTrue(_windowQueue.Contains(secondWindowMock.Object));
			Assert.IsTrue(_windowQueue.Contains(thirdWindowMock.Object));
			Assert.IsTrue(_windowQueue.Contains(fourWindowMock.Object));
			Assert.AreEqual(_windowQueue.Head.Data, secondWindowMock.Object);
			Assert.AreEqual(_windowQueue.Tail.Data, firstWindowMock.Object);
			Assert.AreEqual(4,                      _windowQueue.Count);
			Assert.AreEqual(4,                      _windowQueue.ListMap.Count);
			TestNode(_windowQueue.Head, new[]
			                            {
				                            secondWindowMock.Object,
				                            fourWindowMock.Object,
				                            thirdWindowMock.Object,
				                            firstWindowMock.Object,
			                            });
		}

		[Test]
		public void AddFirstFourWithDoubleThirdTest()
		{
			var firstWindowMock  = CreateWindowMock<TestWindowTypeOne>();
			var secondWindowMock = CreateWindowMock<TestWindowTypeTwo>();
			var thirdWindowMock  = CreateWindowMock<TestWindowTypeThree>();
			var fourWindowMock   = CreateWindowMock<TestWindowTypeFour>();
			_windowQueue.AddFirst(firstWindowMock.Object);
			_windowQueue.AddFirst(secondWindowMock.Object);
			_windowQueue.AddFirst(thirdWindowMock.Object);
			_windowQueue.AddFirst(fourWindowMock.Object);
			_windowQueue.AddFirst(thirdWindowMock.Object);
			Assert.IsFalse(_windowQueue.IsEmpty);
			Assert.IsTrue(_windowQueue.Contains(firstWindowMock.Object));
			Assert.IsTrue(_windowQueue.Contains(secondWindowMock.Object));
			Assert.IsTrue(_windowQueue.Contains(thirdWindowMock.Object));
			Assert.IsTrue(_windowQueue.Contains(fourWindowMock.Object));
			Assert.AreEqual(_windowQueue.Head.Data, thirdWindowMock.Object);
			Assert.AreEqual(_windowQueue.Tail.Data, firstWindowMock.Object);
			Assert.AreEqual(4,                      _windowQueue.Count);
			Assert.AreEqual(4,                      _windowQueue.ListMap.Count);
			TestNode(_windowQueue.Head, new[]
			                            {
				                            thirdWindowMock.Object,
				                            fourWindowMock.Object,
				                            secondWindowMock.Object,
				                            firstWindowMock.Object,
			                            });
		}

		[Test]
		public void AddFirstFourWithDoubleFourthTest()
		{
			var firstWindowMock  = CreateWindowMock<TestWindowTypeOne>();
			var secondWindowMock = CreateWindowMock<TestWindowTypeTwo>();
			var thirdWindowMock  = CreateWindowMock<TestWindowTypeThree>();
			var fourWindowMock   = CreateWindowMock<TestWindowTypeFour>();
			_windowQueue.AddFirst(firstWindowMock.Object);
			_windowQueue.AddFirst(secondWindowMock.Object);
			_windowQueue.AddFirst(thirdWindowMock.Object);
			_windowQueue.AddFirst(fourWindowMock.Object);
			_windowQueue.AddFirst(fourWindowMock.Object);
			Assert.IsFalse(_windowQueue.IsEmpty);
			Assert.IsTrue(_windowQueue.Contains(firstWindowMock.Object));
			Assert.IsTrue(_windowQueue.Contains(secondWindowMock.Object));
			Assert.IsTrue(_windowQueue.Contains(thirdWindowMock.Object));
			Assert.IsTrue(_windowQueue.Contains(fourWindowMock.Object));
			Assert.AreEqual(_windowQueue.Head.Data, fourWindowMock.Object);
			Assert.AreEqual(_windowQueue.Tail.Data, firstWindowMock.Object);
			Assert.AreEqual(4,                      _windowQueue.Count);
			Assert.AreEqual(4,                      _windowQueue.ListMap.Count);
			TestNode(_windowQueue.Head, new[]
			                            {
				                            fourWindowMock.Object,
				                            thirdWindowMock.Object,
				                            secondWindowMock.Object,
				                            firstWindowMock.Object,
			                            });
		}

		[Test]
		public void AddLastOneTest()
		{
			var windowMock = CreateWindowMock<TestWindowTypeOne>();
			_windowQueue.AddLast(windowMock.Object);
			Assert.IsFalse(_windowQueue.IsEmpty);
			Assert.IsTrue(_windowQueue.Contains(windowMock.Object));
			Assert.AreEqual(_windowQueue.Head.Data, windowMock.Object);
			Assert.AreEqual(_windowQueue.Tail.Data, windowMock.Object);
			Assert.AreEqual(1,                      _windowQueue.Count);
			Assert.AreEqual(1,                      _windowQueue.ListMap.Count);
			TestNode(_windowQueue.Head, new[] { windowMock.Object });
		}

		[Test]
		public void AddLastTwoTest()
		{
			var firstWindowMock  = CreateWindowMock<TestWindowTypeOne>();
			var secondWindowMock = CreateWindowMock<TestWindowTypeTwo>();
			_windowQueue.AddLast(firstWindowMock.Object);
			_windowQueue.AddLast(secondWindowMock.Object);
			Assert.IsFalse(_windowQueue.IsEmpty);
			Assert.IsTrue(_windowQueue.Contains(firstWindowMock.Object));
			Assert.IsTrue(_windowQueue.Contains(secondWindowMock.Object));
			Assert.AreEqual(_windowQueue.Head.Data, firstWindowMock.Object);
			Assert.AreEqual(_windowQueue.Tail.Data, secondWindowMock.Object);
			Assert.AreEqual(2,                      _windowQueue.Count);
			Assert.AreEqual(2,                      _windowQueue.ListMap.Count);
			TestNode(_windowQueue.Head, new[]
			                            {
				                            firstWindowMock.Object,
				                            secondWindowMock.Object,
			                            });
		}

		[Test]
		public void AddLastTwoWithDoubleFirstTest()
		{
			var firstWindowMock  = CreateWindowMock<TestWindowTypeOne>();
			var secondWindowMock = CreateWindowMock<TestWindowTypeTwo>();
			_windowQueue.AddLast(firstWindowMock.Object);
			_windowQueue.AddLast(secondWindowMock.Object);
			_windowQueue.AddLast(firstWindowMock.Object);
			Assert.IsFalse(_windowQueue.IsEmpty);
			Assert.IsTrue(_windowQueue.Contains(firstWindowMock.Object));
			Assert.IsTrue(_windowQueue.Contains(secondWindowMock.Object));
			Assert.AreEqual(_windowQueue.Head.Data, secondWindowMock.Object);
			Assert.AreEqual(_windowQueue.Tail.Data, firstWindowMock.Object);
			Assert.AreEqual(2,                      _windowQueue.Count);
			Assert.AreEqual(2,                      _windowQueue.ListMap.Count);
			TestNode(_windowQueue.Head, new[]
			                            {
				                            secondWindowMock.Object,
				                            firstWindowMock.Object,
			                            });
		}

		[Test]
		public void AddLastFourWithDoubleFirstTest()
		{
			var firstWindowMock  = CreateWindowMock<TestWindowTypeOne>();
			var secondWindowMock = CreateWindowMock<TestWindowTypeTwo>();
			var thirdWindowMock  = CreateWindowMock<TestWindowTypeThree>();
			var fourWindowMock   = CreateWindowMock<TestWindowTypeFour>();
			_windowQueue.AddLast(firstWindowMock.Object);
			_windowQueue.AddLast(secondWindowMock.Object);
			_windowQueue.AddLast(thirdWindowMock.Object);
			_windowQueue.AddLast(fourWindowMock.Object);
			_windowQueue.AddLast(firstWindowMock.Object);
			Assert.IsFalse(_windowQueue.IsEmpty);
			Assert.IsTrue(_windowQueue.Contains(firstWindowMock.Object));
			Assert.IsTrue(_windowQueue.Contains(secondWindowMock.Object));
			Assert.IsTrue(_windowQueue.Contains(thirdWindowMock.Object));
			Assert.IsTrue(_windowQueue.Contains(fourWindowMock.Object));
			Assert.AreEqual(_windowQueue.Head.Data, secondWindowMock.Object);
			Assert.AreEqual(_windowQueue.Tail.Data, firstWindowMock.Object);
			Assert.AreEqual(4,                      _windowQueue.Count);
			Assert.AreEqual(4,                      _windowQueue.ListMap.Count);
			TestNode(_windowQueue.Head, new[]
			                            {
				                            secondWindowMock.Object,
				                            thirdWindowMock.Object,
				                            fourWindowMock.Object,
				                            firstWindowMock.Object,
			                            });
		}

		[Test]
		public void AddLastFourWithDoubleSecondTest()
		{
			var firstWindowMock  = CreateWindowMock<TestWindowTypeOne>();
			var secondWindowMock = CreateWindowMock<TestWindowTypeTwo>();
			var thirdWindowMock  = CreateWindowMock<TestWindowTypeThree>();
			var fourWindowMock   = CreateWindowMock<TestWindowTypeFour>();
			_windowQueue.AddLast(firstWindowMock.Object);
			_windowQueue.AddLast(secondWindowMock.Object);
			_windowQueue.AddLast(thirdWindowMock.Object);
			_windowQueue.AddLast(fourWindowMock.Object);
			_windowQueue.AddLast(secondWindowMock.Object);
			Assert.IsFalse(_windowQueue.IsEmpty);
			Assert.IsTrue(_windowQueue.Contains(firstWindowMock.Object));
			Assert.IsTrue(_windowQueue.Contains(secondWindowMock.Object));
			Assert.IsTrue(_windowQueue.Contains(thirdWindowMock.Object));
			Assert.IsTrue(_windowQueue.Contains(fourWindowMock.Object));
			Assert.AreEqual(_windowQueue.Head.Data, firstWindowMock.Object);
			Assert.AreEqual(_windowQueue.Tail.Data, secondWindowMock.Object);
			Assert.AreEqual(4,                      _windowQueue.Count);
			Assert.AreEqual(4,                      _windowQueue.ListMap.Count);
			TestNode(_windowQueue.Head, new[]
			                            {
				                            firstWindowMock.Object,
				                            thirdWindowMock.Object,
				                            fourWindowMock.Object,
				                            secondWindowMock.Object,
			                            });
		}

		[Test]
		public void AddLastFourWithDoubleThirdTest()
		{
			var firstWindowMock  = CreateWindowMock<TestWindowTypeOne>();
			var secondWindowMock = CreateWindowMock<TestWindowTypeTwo>();
			var thirdWindowMock  = CreateWindowMock<TestWindowTypeThree>();
			var fourWindowMock   = CreateWindowMock<TestWindowTypeFour>();
			_windowQueue.AddLast(firstWindowMock.Object);
			_windowQueue.AddLast(secondWindowMock.Object);
			_windowQueue.AddLast(thirdWindowMock.Object);
			_windowQueue.AddLast(fourWindowMock.Object);
			_windowQueue.AddLast(thirdWindowMock.Object);
			Assert.IsFalse(_windowQueue.IsEmpty);
			Assert.IsTrue(_windowQueue.Contains(firstWindowMock.Object));
			Assert.IsTrue(_windowQueue.Contains(secondWindowMock.Object));
			Assert.IsTrue(_windowQueue.Contains(thirdWindowMock.Object));
			Assert.IsTrue(_windowQueue.Contains(fourWindowMock.Object));
			Assert.AreEqual(_windowQueue.Head.Data, firstWindowMock.Object);
			Assert.AreEqual(_windowQueue.Tail.Data, thirdWindowMock.Object);
			Assert.AreEqual(4,                      _windowQueue.Count);
			Assert.AreEqual(4,                      _windowQueue.ListMap.Count);
			TestNode(_windowQueue.Head, new[]
			                            {
				                            firstWindowMock.Object,
				                            secondWindowMock.Object,
				                            fourWindowMock.Object,
				                            thirdWindowMock.Object,
			                            });
		}

		[Test]
		public void AddLastFourWithDoubleFourthTest()
		{
			var firstWindowMock  = CreateWindowMock<TestWindowTypeOne>();
			var secondWindowMock = CreateWindowMock<TestWindowTypeTwo>();
			var thirdWindowMock  = CreateWindowMock<TestWindowTypeThree>();
			var fourWindowMock   = CreateWindowMock<TestWindowTypeFour>();
			_windowQueue.AddLast(firstWindowMock.Object);
			_windowQueue.AddLast(secondWindowMock.Object);
			_windowQueue.AddLast(thirdWindowMock.Object);
			_windowQueue.AddLast(fourWindowMock.Object);
			_windowQueue.AddLast(fourWindowMock.Object);
			Assert.IsFalse(_windowQueue.IsEmpty);
			Assert.IsTrue(_windowQueue.Contains(firstWindowMock.Object));
			Assert.IsTrue(_windowQueue.Contains(secondWindowMock.Object));
			Assert.IsTrue(_windowQueue.Contains(thirdWindowMock.Object));
			Assert.IsTrue(_windowQueue.Contains(fourWindowMock.Object));
			Assert.AreEqual(_windowQueue.Head.Data, firstWindowMock.Object);
			Assert.AreEqual(_windowQueue.Tail.Data, fourWindowMock.Object);
			Assert.AreEqual(4,                      _windowQueue.Count);
			Assert.AreEqual(4,                      _windowQueue.ListMap.Count);
			TestNode(_windowQueue.Head, new[]
			                            {
				                            firstWindowMock.Object,
				                            secondWindowMock.Object,
				                            thirdWindowMock.Object,
				                            fourWindowMock.Object,
			                            });
		}

		[Test]
		public void RemoveTest()
		{
			var firstWindowMock  = CreateWindowMock<TestWindowTypeOne>();
			var secondWindowMock = CreateWindowMock<TestWindowTypeTwo>();
			var thirdWindowMock  = CreateWindowMock<TestWindowTypeThree>();
			var fourWindowMock   = CreateWindowMock<TestWindowTypeFour>();
			_windowQueue.AddLast(firstWindowMock.Object);
			_windowQueue.AddLast(secondWindowMock.Object);
			_windowQueue.AddLast(thirdWindowMock.Object);
			_windowQueue.AddLast(fourWindowMock.Object);
			var removed = _windowQueue.RemoveFirst();
			Assert.IsFalse(_windowQueue.IsEmpty);
			Assert.IsFalse(_windowQueue.Contains(firstWindowMock.Object));
			Assert.IsTrue(_windowQueue.Contains(secondWindowMock.Object));
			Assert.IsTrue(_windowQueue.Contains(thirdWindowMock.Object));
			Assert.IsTrue(_windowQueue.Contains(fourWindowMock.Object));
			Assert.AreEqual(firstWindowMock.Object, removed);
			Assert.AreEqual(_windowQueue.Head.Data, secondWindowMock.Object);
			Assert.AreEqual(_windowQueue.Tail.Data, fourWindowMock.Object);
			Assert.AreEqual(3,                      _windowQueue.Count);
			Assert.AreEqual(3,                      _windowQueue.ListMap.Count);
			TestNode(_windowQueue.Head, new[]
			                            {
				                            secondWindowMock.Object,
				                            thirdWindowMock.Object,
				                            fourWindowMock.Object,
			                            });
		}

		private static Mock<IWindow> CreateWindowMock<T>()
		{
			var firstWindowMock = new Mock<IWindow>();
			firstWindowMock.SetupGet(window => window.WindowType).Returns(typeof(T));
			return firstWindowMock;
		}

		private static void TestNode(DoublyNode<IWindow> node, IWindow[] windows)
		{
			var head = node;
			Assert.IsNull(head.Previous);
			var i = 0;
			while (head != null)
			{
				Assert.AreEqual(windows[i++], head.Data);
				head = head.Next;
			}

			Assert.AreEqual(i, windows.Length);
		}

		[UsedImplicitly]
		private class TestWindowTypeOne { }

		[UsedImplicitly]
		private class TestWindowTypeTwo { }

		[UsedImplicitly]
		private class TestWindowTypeThree { }

		[UsedImplicitly]
		private class TestWindowTypeFour { }
	}
}