using System;
using Inventory;
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
			var increment     = true;
			var inventoryPack = new InventoryPack();
			inventoryPack.Initialize(InventoryPackModel.GetTestModel(), 10);
			inventoryPack.PackIsEmpty += () =>
			{
				increment = false;
				Assert.Pass();
			};
			while (increment)
			{
				inventoryPack.RemoveItem();
			}
		}

		[Test]
		public void PackSizeChangedEventTest()
		{
			var testSize      = 10;
			var inventoryPack = new InventoryPack();
			inventoryPack.Initialize(InventoryPackModel.GetTestModel(), testSize);
			IDisposable disposable = null;
			disposable = inventoryPack.Size.Subscribe(i =>
			{
				// ReSharper disable once AccessToModifiedClosure
				Assert.AreEqual(testSize, i);
				// ReSharper disable once AccessToModifiedClosure
				disposable?.Dispose();
			});
			testSize = 9;
			inventoryPack.RemoveItem();
		}
	}
}