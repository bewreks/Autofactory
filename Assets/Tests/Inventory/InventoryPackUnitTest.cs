using System;
using Factories;
using Inventories;
using NUnit.Framework;
using UniRx;
using Zenject;

namespace Tests.Inventory
{
	[TestFixture]
	public class InventoryPackUnitTest : ZenjectUnitTestFixture
	{
		public class InventoryPackTestData
		{
			public int                packSizeInit;
			public int                packSizeResult;
			public int                packSizeEdge;
			public int                toChange;
			public InventoryPackModel Model;
		}

		private static InventoryPackTestData[] initTestDatas =
		{
			new InventoryPackTestData
			{
				packSizeInit   = -1,
				packSizeEdge   = 0,
				packSizeResult = 0,
				Model          = InventoryPackModel.GetTestModel()
			},
			new InventoryPackTestData
			{
				packSizeInit   = 0,
				packSizeEdge   = 0,
				packSizeResult = 0,
				Model          = InventoryPackModel.GetTestModel()
			},
			new InventoryPackTestData
			{
				packSizeInit   = 1,
				packSizeEdge   = 0,
				packSizeResult = 1,
				Model          = InventoryPackModel.GetTestModel()
			},
			new InventoryPackTestData
			{
				packSizeInit   = 50,
				packSizeEdge   = 0,
				packSizeResult = 50,
				Model          = InventoryPackModel.GetTestModel()
			},
			new InventoryPackTestData
			{
				packSizeInit   = 51,
				packSizeEdge   = 1,
				packSizeResult = 50,
				Model          = InventoryPackModel.GetTestModel()
			},
		};

		private static InventoryPackTestData[] addTestDatas =
		{
			new InventoryPackTestData
			{
				packSizeInit   = 1,
				packSizeEdge   = 0,
				packSizeResult = 1,
				toChange       = -1,
				Model          = InventoryPackModel.GetTestModel()
			},
			new InventoryPackTestData
			{
				packSizeInit   = 1,
				packSizeEdge   = 0,
				packSizeResult = 1,
				toChange       = 0,
				Model          = InventoryPackModel.GetTestModel()
			},
			new InventoryPackTestData
			{
				packSizeInit   = 1,
				packSizeEdge   = 0,
				packSizeResult = 2,
				toChange       = 1,
				Model          = InventoryPackModel.GetTestModel()
			},
			new InventoryPackTestData
			{
				packSizeInit   = 1,
				packSizeEdge   = 0,
				packSizeResult = 50,
				toChange       = 49,
				Model          = InventoryPackModel.GetTestModel()
			},
			new InventoryPackTestData
			{
				packSizeInit   = 1,
				packSizeEdge   = 1,
				packSizeResult = 50,
				toChange       = 50,
				Model          = InventoryPackModel.GetTestModel()
			},
			new InventoryPackTestData
			{
				packSizeInit   = 50,
				packSizeEdge   = 1,
				packSizeResult = 50,
				toChange       = 1,
				Model          = InventoryPackModel.GetTestModel()
			}
		};

		private static InventoryPackTestData[] removeTestDatas =
		{
			new InventoryPackTestData
			{
				packSizeInit   = 1,
				packSizeEdge   = 0,
				packSizeResult = 1,
				toChange       = -1,
				Model          = InventoryPackModel.GetTestModel()
			},
			new InventoryPackTestData
			{
				packSizeInit   = 1,
				packSizeEdge   = 0,
				packSizeResult = 1,
				toChange       = 0,
				Model          = InventoryPackModel.GetTestModel()
			},
			new InventoryPackTestData
			{
				packSizeInit   = 2,
				packSizeEdge   = 0,
				packSizeResult = 1,
				toChange       = 1,
				Model          = InventoryPackModel.GetTestModel()
			},
			new InventoryPackTestData
			{
				packSizeInit   = 50,
				packSizeEdge   = 0,
				packSizeResult = 0,
				toChange       = 50,
				Model          = InventoryPackModel.GetTestModel()
			},
			new InventoryPackTestData
			{
				packSizeInit   = 50,
				packSizeEdge   = 1,
				packSizeResult = 0,
				toChange       = 51,
				Model          = InventoryPackModel.GetTestModel()
			},
			new InventoryPackTestData
			{
				packSizeInit   = 0,
				packSizeEdge   = 1,
				packSizeResult = 0,
				toChange       = 1,
				Model          = InventoryPackModel.GetTestModel()
			}
		};

		private static InventoryPackTestData[] setTestDatas =
		{
			new InventoryPackTestData
			{
				packSizeInit   = 1,
				packSizeEdge   = 0,
				packSizeResult = 0,
				toChange       = -1,
				Model          = InventoryPackModel.GetTestModel()
			},
			new InventoryPackTestData
			{
				packSizeInit   = 1,
				packSizeEdge   = 0,
				packSizeResult = 0,
				toChange       = 0,
				Model          = InventoryPackModel.GetTestModel()
			},
			new InventoryPackTestData
			{
				packSizeInit   = 2,
				packSizeEdge   = 0,
				packSizeResult = 1,
				toChange       = 1,
				Model          = InventoryPackModel.GetTestModel()
			},
			new InventoryPackTestData
			{
				packSizeInit   = 1,
				packSizeEdge   = 0,
				packSizeResult = 50,
				toChange       = 50,
				Model          = InventoryPackModel.GetTestModel()
			},
			new InventoryPackTestData
			{
				packSizeInit   = 1,
				packSizeEdge   = 1,
				packSizeResult = 50,
				toChange       = 51,
				Model          = InventoryPackModel.GetTestModel()
			},
		};

		[Test]
		public void InitTest([ValueSource("initTestDatas")] InventoryPackTestData data)
		{
			var inventoryPack = new InventoryPack();
			var initialize    = 0;
			try
			{
				initialize = inventoryPack.Initialize(data.Model, data.packSizeInit);
			}
			catch (Exception e)
			{
				Assert.Fail($"Fail while pack init \r\n {e.Message}");
			}

			Assert.AreEqual(data.packSizeEdge,   initialize);
			Assert.AreEqual(data.packSizeResult, inventoryPack.Size.Value);
		}

		[Test]
		public void AddTest([ValueSource("addTestDatas")] InventoryPackTestData data)
		{
			var inventoryPack = new InventoryPack();
			var addItem       = 0;
			try
			{
				inventoryPack.Initialize(data.Model, data.packSizeInit);
				addItem = inventoryPack.AddItem(data.toChange);
			}
			catch (Exception e)
			{
				Assert.Fail($"Fail while pack add \r\n {e.Message}");
			}

			Assert.AreEqual(data.packSizeEdge,   addItem);
			Assert.AreEqual(data.packSizeResult, inventoryPack.Size.Value);
		}

		[Test]
		public void RemoveTest([ValueSource("removeTestDatas")] InventoryPackTestData data)
		{
			var inventoryPack = new InventoryPack();
			var removeItem    = 0;
			try
			{
				inventoryPack.Initialize(data.Model, data.packSizeInit);
				removeItem = inventoryPack.RemoveItem(data.toChange);
			}
			catch (Exception e)
			{
				Assert.Fail($"Fail while pack add \r\n {e.Message}");
			}

			Assert.AreEqual(data.packSizeEdge,   removeItem);
			Assert.AreEqual(data.packSizeResult, inventoryPack.Size.Value);
		}

		[Test]
		public void SetTest([ValueSource("setTestDatas")] InventoryPackTestData data)
		{
			var inventoryPack = new InventoryPack();
			var setItem       = 0;
			try
			{
				inventoryPack.Initialize(data.Model, data.packSizeInit);
				setItem = inventoryPack.SetCount(data.toChange);
			}
			catch (Exception e)
			{
				Assert.Fail($"Fail while pack add \r\n {e.Message}");
			}

			Assert.AreEqual(data.packSizeEdge,   setItem);
			Assert.AreEqual(data.packSizeResult, inventoryPack.Size.Value);
		}

		[Test]
		public void PackFullEventTest()
		{
			var increment     = true;
			var inventoryPack = new InventoryPack();
			inventoryPack.Initialize(InventoryPackModel.GetTestModel(), 0);
			inventoryPack.PackIsFull += () =>
			{
				increment = false;
				Assert.Pass();
			};
			while (increment)
			{
				inventoryPack.AddItem();
			}
		}

		[Test]
		public void PackEmptyEventTest()
		{
			CheckIsEmptyEventTest(pack =>
			{
				while (pack.Size.Value != 0)
				{
					pack.RemoveItem();
				}
			}, 1);
		}

		[Test]
		public void PackSetEmptyEventTest()
		{
			CheckIsEmptyEventTest(pack => { pack.SetCount(0); }, 1);
		}

		[Test]
		public void PackSizeAddOneChangedEventTest()
		{
			CheckChangeEventTest(pack => { pack.AddItem(); }, 2, 1);
		}

		[Test]
		public void PackSizeAddNegativeChangedEventTest()
		{
			CheckChangeEventTest(pack => { pack.AddItem(-1); }, 1, 0);
		}

		[Test]
		public void PackSizeAddZeroChangedEventTest()
		{
			CheckChangeEventTest(pack => { pack.AddItem(0); }, 1, 0);
		}

		[Test]
		public void PackSizeSetOneChangedEventTest()
		{
			CheckChangeEventTest(pack => { pack.SetCount(1); }, 2, -9, 10);
		}

		[Test]
		public void PackSizeReSetOneChangedEventTest()
		{
			CheckChangeEventTest(pack => { pack.SetCount(1); }, 1, 0, 1);
		}

		[Test]
		public void PackSizeSetNegativeChangedEventTest()
		{
			CheckChangeEventTest(pack => { pack.SetCount(-1); }, 2, -1);
		}

		[Test]
		public void PackSizeSetZeroChangedEventTest()
		{
			CheckChangeEventTest(pack => { pack.SetCount(0); }, 2, -1);
		}

		[Test]
		public void PackSizeRemoveOneChangedEventTest()
		{
			CheckChangeEventTest(pack => { pack.RemoveItem(); }, 2, -1);
		}

		[Test]
		public void PackSizeRemoveNegativeChangedEventTest()
		{
			CheckChangeEventTest(pack => { pack.RemoveItem(-1); }, 1, 0);
		}

		[Test]
		public void PackSizeRemoveZeroChangedEventTest()
		{
			CheckChangeEventTest(pack => { pack.RemoveItem(0); }, 1, 0);
		}

		private static void CheckIsEmptyEventTest(Action<InventoryPack> func,
		                                          int                   expectedEventCount)
		{
			var eventCount = 0;
			var pack       = new InventoryPack();
			pack.Initialize(InventoryPackModel.GetTestModel(), 10);
			pack.PackIsEmpty += () => { eventCount++; };
			func(pack);
			Assert.AreEqual(expectedEventCount, eventCount);
		}

		private static void CheckChangeEventTest(Action<InventoryPack> func,
		                                         int                   expectedEventCount,
		                                         int                   expectedSizeChange,
		                                         int                   initSize = 1)
		{
			var eventCount = 0;
			var initialize = new InventoryPack();
			var checkSize  = initSize;
			initialize.Initialize(InventoryPackModel.GetTestModel(), initSize);
			IDisposable disposable = null;
			disposable = initialize.Size.Subscribe(i =>
			{
				// ReSharper disable once AccessToModifiedClosure
				Assert.AreEqual(checkSize, i);
				// ReSharper disable once AccessToModifiedClosure
				disposable?.Dispose();
				eventCount++;
			});

			checkSize += expectedSizeChange;
			func(initialize);
			Assert.AreEqual(expectedEventCount, eventCount);
		}

		[Test]
		public void InitNullModelTest()
		{
			var inventoryPack = new InventoryPack();
			try
			{
				inventoryPack.Initialize(null, 0);
			}
			catch (Exception)
			{
				Assert.Pass();
			}

			Assert.Fail();
		}

		[Test]
		public void DisposeTest()
		{
			var inventoryPack = new InventoryPack();
			inventoryPack.Initialize(InventoryPackModel.GetTestModel(), 5);
			Factory.Clear();
			Assert.Zero(Factory.GetCountOf<InventoryPack>());
			inventoryPack.Dispose();
			Assert.Null(inventoryPack.Model);
			Assert.Zero(inventoryPack.Size.Value);
			Assert.NotZero(Factory.GetCountOf<InventoryPack>());
		}
	}
}