using System;
using System.Collections.Generic;
using System.Linq;
using Factories;
using Inventories;
using NUnit.Framework;
using UniRx;
using Zenject;

namespace Tests.Inventory
{
	[TestFixture]
	public class FullInventoryPackUnitTest : ZenjectUnitTestFixture
	{
		#region Test Data classes

		public class InventoryPackInitTestData : InventoryPackTestData
		{
			public int ToChange;
		}

		public class InventoryPackTestData
		{
			public InventoryPackModel         Model;
			public IEnumerable<InventoryPack> PackInit;
			public int                        GetPacksCount;
			public int                        SizeOfPacks;
			public int                        ResultCount;
			public int                        ResultPacksCount;
			public int                        ResultEdge;

			public virtual void PrepareInitPack(FullInventoryPack pack)
			{
				if (PackInit == null && GetPacksCount > 0)
				{
					PackInit = GetTestArray(GetPacksCount, SizeOfPacks);
				}
			}

			public IEnumerable<InventoryPack> GetTestArray(int dataGetPacksCount, int size)
			{
				var inventoryPacks = new InventoryPack[dataGetPacksCount];

				for (var i = 0; i < inventoryPacks.Length; i++)
				{
					inventoryPacks[i] = new InventoryPack();
					inventoryPacks[i].Initialize(Model, size);
				}

				return inventoryPacks;
			}
		}

		public class InventoryPackInsideTestData : InventoryPackTestData
		{
			public IEnumerable<InventoryPack> PackToRemove;

			public override void PrepareInitPack(FullInventoryPack pack)
			{
				base.PrepareInitPack(pack);

				PackToRemove = PackInit;
			}
		}

		public class InventoryPackOutsideTestData : InventoryPackInsideTestData
		{
			public override void PrepareInitPack(FullInventoryPack pack)
			{
				if (PackInit == null && GetPacksCount > 0)
				{
					PackInit = GetTestArray(GetPacksCount, SizeOfPacks);
				}

				PackToRemove = GetTestArray(GetPacksCount, SizeOfPacks);
			}
		}

		public class InventoryPackAddFullTestData : InventoryPackTestData
		{
			public FullInventoryPack PackToAdd;
			public int               NewPacksCount;
			public int               NewSizeOfPacks;

			public override void PrepareInitPack(FullInventoryPack pack)
			{
				base.PrepareInitPack(pack);
				PreparePackToAdd(pack);
			}

			protected virtual void PreparePackToAdd(FullInventoryPack pack)
			{
				PackToAdd = new FullInventoryPack();
				PackToAdd.Initialize(GetTestArray(NewPacksCount, NewSizeOfPacks), Model);
			}
		}

		public class InventoryPackAddSelfFullTestData : InventoryPackAddFullTestData
		{
			protected override void PreparePackToAdd(FullInventoryPack pack)
			{
				PackToAdd = pack;
			}
		}

		#endregion

		#region Test datas

		private static InventoryPackInitTestData[] initTestDatas =
		{
			new InventoryPackInitTestData
			{
				Model            = InventoryPackModel.GetTestModel(),
				PackInit         = null,
				ResultCount      = 0,
				ResultPacksCount = 0
			},
			new InventoryPackInitTestData
			{
				Model            = InventoryPackModel.GetTestModel(),
				PackInit         = new List<InventoryPack>(),
				ResultCount      = 0,
				ResultPacksCount = 0
			},
			new InventoryPackInitTestData
			{
				Model            = InventoryPackModel.GetTestModel(),
				PackInit         = Array.Empty<InventoryPack>(),
				ResultCount      = 0,
				ResultPacksCount = 0
			},
			new InventoryPackInitTestData
			{
				Model            = InventoryPackModel.GetTestModel(),
				PackInit         = null,
				GetPacksCount    = 5,
				SizeOfPacks      = 1,
				ResultCount      = 5,
				ResultPacksCount = 1
			},
		};

		private static InventoryPackInitTestData[] addTestDatas =
		{
			new InventoryPackInitTestData
			{
				Model            = InventoryPackModel.GetTestModel(),
				PackInit         = null,
				ResultPacksCount = 0,
				ResultCount      = 0,
				ToChange         = -1
			},
			new InventoryPackInitTestData
			{
				Model            = InventoryPackModel.GetTestModel(),
				PackInit         = null,
				ResultPacksCount = 0,
				ResultCount      = 0,
				ToChange         = 0
			},
			new InventoryPackInitTestData
			{
				Model            = InventoryPackModel.GetTestModel(),
				PackInit         = null,
				ResultPacksCount = 1,
				ResultCount      = 1,
				ToChange         = 1
			},
			new InventoryPackInitTestData
			{
				Model            = InventoryPackModel.GetTestModel(),
				PackInit         = null,
				ResultPacksCount = 1,
				ResultCount      = 50,
				ToChange         = 50
			},
			new InventoryPackInitTestData
			{
				Model            = InventoryPackModel.GetTestModel(),
				PackInit         = null,
				ResultPacksCount = 2,
				ResultCount      = 51,
				ToChange         = 51
			},
			new InventoryPackInitTestData
			{
				Model            = InventoryPackModel.GetTestModel(),
				PackInit         = null,
				ResultPacksCount = 1,
				ResultCount      = 5,
				GetPacksCount    = 5,
				SizeOfPacks      = 1
			},
		};

		private static InventoryPackInitTestData[] removeTestDatas =
		{
			new InventoryPackInitTestData
			{
				Model            = InventoryPackModel.GetTestModel(),
				PackInit         = null,
				ToChange         = -1,
				ResultCount      = 0,
				ResultPacksCount = 0,
			},
			new InventoryPackInitTestData
			{
				Model            = InventoryPackModel.GetTestModel(),
				GetPacksCount    = 1,
				SizeOfPacks      = 50,
				ToChange         = -1,
				ResultCount      = 50,
				ResultPacksCount = 1,
			},
			new InventoryPackInitTestData
			{
				Model            = InventoryPackModel.GetTestModel(),
				GetPacksCount    = 1,
				SizeOfPacks      = 50,
				ToChange         = 0,
				ResultCount      = 50,
				ResultPacksCount = 1,
			},
			new InventoryPackInitTestData
			{
				Model            = InventoryPackModel.GetTestModel(),
				GetPacksCount    = 1,
				SizeOfPacks      = 50,
				ToChange         = 1,
				ResultCount      = 49,
				ResultPacksCount = 1,
			},
			new InventoryPackInitTestData
			{
				Model            = InventoryPackModel.GetTestModel(),
				GetPacksCount    = 1,
				SizeOfPacks      = 50,
				ToChange         = 50,
				ResultCount      = 0,
				ResultPacksCount = 0,
			},
			new InventoryPackInitTestData
			{
				Model            = InventoryPackModel.GetTestModel(),
				GetPacksCount    = 1,
				SizeOfPacks      = 50,
				ToChange         = 51,
				ResultCount      = 0,
				ResultPacksCount = 0,
				ResultEdge       = 1
			},
		};

		private static InventoryPackInsideTestData[] removePacksTestDatas =
		{
			new InventoryPackInsideTestData
			{
				Model            = InventoryPackModel.GetTestModel(),
				GetPacksCount    = 1,
				SizeOfPacks      = 50,
				PackInit         = null,
				ResultCount      = 0,
				ResultPacksCount = 0,
			},
			new InventoryPackOutsideTestData
			{
				Model            = InventoryPackModel.GetTestModel(),
				GetPacksCount    = 1,
				SizeOfPacks      = 50,
				PackInit         = null,
				ResultCount      = 50,
				ResultPacksCount = 1,
			},
		};

		private static InventoryPackAddFullTestData[] addFullPackTestDatas =
		{
			new InventoryPackAddFullTestData
			{
				Model            = InventoryPackModel.GetTestModel(),
				GetPacksCount    = 1,
				SizeOfPacks      = 1,
				NewPacksCount    = 1,
				NewSizeOfPacks   = 1,
				ResultCount      = 2,
				ResultPacksCount = 1
			},
			new InventoryPackAddFullTestData
			{
				Model            = InventoryPackModel.GetTestModel(),
				GetPacksCount    = 1,
				SizeOfPacks      = 50,
				NewPacksCount    = 1,
				NewSizeOfPacks   = 1,
				ResultCount      = 51,
				ResultPacksCount = 2
			},
			new InventoryPackAddFullTestData
			{
				Model            = InventoryPackModel.GetTestModel(),
				GetPacksCount    = 1,
				SizeOfPacks      = 1,
				NewPacksCount    = 1,
				NewSizeOfPacks   = 50,
				ResultCount      = 51,
				ResultPacksCount = 2
			},
			new InventoryPackAddSelfFullTestData
			{
				Model            = InventoryPackModel.GetTestModel(),
				GetPacksCount    = 1,
				SizeOfPacks      = 1,
				ResultCount      = 1,
				ResultPacksCount = 1
			}
		};
		#endregion

		[Test]
		public void InitTest([ValueSource("initTestDatas")] InventoryPackInitTestData data)
		{
			var pack = new FullInventoryPack();
			data.PrepareInitPack(pack);

			try
			{
				pack.Initialize(data.PackInit, data.Model);
			}
			catch (Exception e)
			{
				Assert.Fail($"Failed init \r\n {e.Message}");
			}

			Assert.AreEqual(data.ResultCount,      pack.Count.Value);
			Assert.AreEqual(data.ResultPacksCount, pack.Packs.Count());
		}

		[Test]
		public void AddTest([ValueSource("addTestDatas")] InventoryPackInitTestData data)
		{
			var fullPack = new FullInventoryPack();

			try
			{
				fullPack.Initialize(data.PackInit, data.Model);

				if (data.GetPacksCount != 0)
				{
					foreach (var pack in data.GetTestArray(data.GetPacksCount, data.SizeOfPacks))
					{
						fullPack.Add(pack);
					}
				}
				else
				{
					fullPack.Add(data.ToChange);
				}
			}
			catch (Exception e)
			{
				Assert.Fail($"Failed init \r\n {e.Message}");
			}

			Assert.AreEqual(data.ResultCount,      fullPack.Count.Value);
			Assert.AreEqual(data.ResultPacksCount, fullPack.Packs.Count());
		}

		[Test]
		public void RemoveTest([ValueSource("removeTestDatas")] InventoryPackInitTestData data)
		{
			var fullPack = new FullInventoryPack();
			var edge     = 0;
			try
			{
				data.PrepareInitPack(fullPack);

				fullPack.Initialize(data.PackInit, data.Model);
				edge = fullPack.Remove(data.ToChange);
			}
			catch (Exception e)
			{
				Assert.Fail($"Failed init \r\n {e.Message}");
			}

			Assert.AreEqual(data.ResultCount,      fullPack.Count.Value);
			Assert.AreEqual(data.ResultPacksCount, fullPack.Packs.Count());
			Assert.AreEqual(data.ResultEdge,       edge);
		}

		[Test]
		public void AddAnotherFullPackTest([ValueSource("addFullPackTestDatas")] InventoryPackAddFullTestData data)
		{
			var fullPack = new FullInventoryPack();
			data.PrepareInitPack(fullPack);
			fullPack.Initialize(data.PackInit, data.Model);
			fullPack.Add(data.PackToAdd);
			Assert.AreEqual(data.ResultCount,      fullPack.Count.Value);
			Assert.AreEqual(data.ResultPacksCount, fullPack.Packs.Count());
		}

		[Test]
		public void RemovePackTest([ValueSource("removePacksTestDatas")] InventoryPackInsideTestData data)
		{
			var fullPack = new FullInventoryPack();
			data.PrepareInitPack(fullPack);
			fullPack.Initialize(data.PackInit, InventoryPackModel.GetTestModel());
			fullPack.Remove(data.PackToRemove.ToArray()[0]);
			Assert.AreEqual(data.ResultCount,      fullPack.Count.Value);
			Assert.AreEqual(data.ResultPacksCount, fullPack.Packs.Count());
		}

		[Test]
		public void InitWithNullModelTest()
		{
			var pack = new FullInventoryPack();
			try
			{
				pack.Initialize(null, null);
			}
			catch (Exception)
			{
				Assert.Pass();
			}

			Assert.Fail();
		}

		[Test]
		public void ClearTest()
		{
			var fullInventoryPack = new FullInventoryPack();
			fullInventoryPack.Initialize(null, InventoryPackModel.GetTestModel());
			fullInventoryPack.Add(2);
			Assert.AreEqual(2, fullInventoryPack.Count.Value);
			fullInventoryPack.Clear();
			Assert.AreEqual(0, fullInventoryPack.Count.Value);
		}

		[Test]
		public void DisposeTest()
		{
			Factory.Clear();
			Assert.Zero(Factory.GetCountOf<FullInventoryPack>());

			var fullInventoryPack = new FullInventoryPack();
			fullInventoryPack.Initialize(null, InventoryPackModel.GetTestModel());
			fullInventoryPack.Dispose();

			Assert.AreEqual(1, Factory.GetCountOf<FullInventoryPack>());
		}

		[Test]
		public void ChangeEventAddOneTest()
		{
			void AddFunc(FullInventoryPack fullPack)
			{
				fullPack.Add();
			}
			CheckEventTest(AddFunc, 2, 1);
		}

		[Test]
		public void ChangeEventAddPackTest()
		{
			void AddFunc(FullInventoryPack fullPack)
			{
				var newPack = new InventoryPack();
				newPack.Initialize(InventoryPackModel.GetTestModel());
				fullPack.Add(newPack);
			}
			CheckEventTest(AddFunc, 2, 1);
		}

		[Test]
		public void ChangeEventAddZeroTest()
		{
			void AddFunc(FullInventoryPack fullPack)
			{
				fullPack.Add(0);
			}
			CheckEventTest(AddFunc, 1, 0);
		}

		[Test]
		public void ChangeEventAddNegativeTest()
		{
			void AddFunc(FullInventoryPack fullPack)
			{
				fullPack.Add(-1);
			}
			CheckEventTest(AddFunc, 1, 0);
		}

		[Test]
		public void ChangeEventAddFullTest()
		{
			void AddFunc(FullInventoryPack fullPack)
			{
				var fullInventoryPack = new FullInventoryPack();
				var newPack           = new InventoryPack();
				fullInventoryPack.Initialize(null, InventoryPackModel.GetTestModel());
				newPack.Initialize(InventoryPackModel.GetTestModel());
				fullInventoryPack.Add(newPack);
				fullPack.Add(fullInventoryPack);
			}
			CheckEventTest(AddFunc, 2, 1);
		}

		[Test]
		public void ChangeEventAddSelfTest()
		{
			void AddFunc(FullInventoryPack fullPack)
			{
				fullPack.Add(fullPack);
			}
			CheckEventTest(AddFunc, 1, 0);
		}
		
		[Test]
		public void ChangeEventRemoveOneTest()
		{
			void AddFunc(FullInventoryPack fullPack)
			{
				fullPack.Remove();
			}
			CheckEventTest(AddFunc, 2, -1);
		}

		[Test]
		public void ChangeEventRemovePackTest()
		{
			void AddFunc(FullInventoryPack fullPack)
			{
				var newPack = new InventoryPack();
				newPack.Initialize(InventoryPackModel.GetTestModel());
				fullPack.Remove(newPack);
			}
			CheckEventTest(AddFunc, 1, 0);
		}

		[Test]
		public void ChangeEventRemoveSpecificPackTest()
		{
			void AddFunc(FullInventoryPack fullPack)
			{
				fullPack.Remove(fullPack.Packs.First());
			}
			CheckEventTest(AddFunc, 2, -1);
		}

		[Test]
		public void ChangeEventRemoveZeroTest()
		{
			void AddFunc(FullInventoryPack fullPack)
			{
				fullPack.Remove(0);
			}
			CheckEventTest(AddFunc, 1, 0);
		}

		[Test]
		public void ChangeEventRemoveNegativeTest()
		{
			void AddFunc(FullInventoryPack fullPack)
			{
				fullPack.Remove(-1);
			}
			CheckEventTest(AddFunc, 1, 0);
		}


		private static void CheckEventTest(Action<FullInventoryPack> func, int expectedEventCount, int expectedSizeChange)
		{
			var eventCount        = 0;
			var fullInventoryPack = new FullInventoryPack();
			var initialize        = new InventoryPack();
			var checkSize         = 1;
			initialize.Initialize(InventoryPackModel.GetTestModel());
			fullInventoryPack.Initialize(new[] { initialize }, InventoryPackModel.GetTestModel());
			IDisposable disposable = null;
			disposable = fullInventoryPack.Count.Subscribe(i =>
			{
				// ReSharper disable once AccessToModifiedClosure
				Assert.AreEqual(checkSize, i);
				// ReSharper disable once AccessToModifiedClosure
				disposable?.Dispose();
				eventCount++;
			});

			checkSize += expectedSizeChange;
			func(fullInventoryPack);
			Assert.AreEqual(expectedEventCount, eventCount);
		}
	}
}