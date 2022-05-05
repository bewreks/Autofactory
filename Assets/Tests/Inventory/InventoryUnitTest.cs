using System;
using System.Linq;
using Factories;
using Installers;
using Inventories;
using NUnit.Framework;
using UnityEngine;
using Zenject;

namespace Tests.Inventory
{
	[TestFixture]
	public class InventoryUnitTest : ZenjectUnitTestFixture
	{
		#region Test Data classes

		public class InventoryTestData
		{
			public InventoryObjectsTypesEnum Type;
			public bool                      ResultChange;
			public int                       ToChange;
			public int                       InitCount;
			public int                       ResultCount;
			public int                       ResultPacksCount;
		}

		public class InventoryLimitTestData
		{
			public InventoryObjectsTypesEnum Type;
			public int                       InitCount;
			public int                       ToChange;
			public bool                      ResultChange;
			public int                       ResultEdge;
		}

		#endregion

		#region Test datas

		private static InventoryTestData[] _addItemTestDatas =
		{
			new InventoryTestData
			{
				Type             = InventoryObjectsTypesEnum.TEST_OBJECT,
				ToChange         = -1,
				ResultCount      = 0,
				ResultPacksCount = 0,
				ResultChange     = false
			},
			new InventoryTestData
			{
				Type             = InventoryObjectsTypesEnum.TEST_OBJECT,
				ToChange         = 0,
				ResultCount      = 0,
				ResultPacksCount = 0,
				ResultChange     = true
			},
			new InventoryTestData
			{
				Type             = InventoryObjectsTypesEnum.TEST_OBJECT,
				ToChange         = 1,
				ResultCount      = 1,
				ResultPacksCount = 1,
				ResultChange     = true
			},
			new InventoryTestData
			{
				Type             = InventoryObjectsTypesEnum.TEST_OBJECT,
				InitCount        = 0,
				ToChange         = 51,
				ResultCount      = 51,
				ResultPacksCount = 2,
				ResultChange     = true
			},
		};

		private static InventoryTestData[] _removeTestData =
		{
			new InventoryTestData
			{
				Type             = InventoryObjectsTypesEnum.TEST_OBJECT,
				InitCount        = 100,
				ResultCount      = 100,
				ResultPacksCount = 2,
				ToChange         = -1,
				ResultChange     = true
			},
			new InventoryTestData
			{
				Type             = InventoryObjectsTypesEnum.TEST_OBJECT,
				InitCount        = 100,
				ResultCount      = 100,
				ResultPacksCount = 2,
				ToChange         = 0,
				ResultChange     = true
			},
			new InventoryTestData
			{
				Type             = InventoryObjectsTypesEnum.TEST_OBJECT,
				InitCount        = 100,
				ResultCount      = 99,
				ResultPacksCount = 2,
				ToChange         = 1,
				ResultChange     = true
			},
			new InventoryTestData
			{
				Type             = InventoryObjectsTypesEnum.TEST_OBJECT,
				InitCount        = 100,
				ResultCount      = 0,
				ResultPacksCount = 0,
				ToChange         = 100,
				ResultChange     = true
			},
			new InventoryTestData
			{
				Type             = InventoryObjectsTypesEnum.TEST_OBJECT,
				InitCount        = 100,
				ResultCount      = 0,
				ResultPacksCount = 0,
				ToChange         = 101,
				ResultChange     = false
			},
			new InventoryTestData
			{
				Type             = InventoryObjectsTypesEnum.TEST_OBJECT,
				InitCount        = 0,
				ResultCount      = 0,
				ResultPacksCount = 0,
				ToChange         = 101,
				ResultChange     = false
			},
		};

		private static InventoryTestData[] _addItemNotConfiguredTestDatas =
		{
			new InventoryTestData
			{
				Type             = InventoryObjectsTypesEnum.TEST_OBJECT,
				ToChange         = -1,
				ResultCount      = 0,
				ResultPacksCount = 0,
				ResultChange     = false
			},
			new InventoryTestData
			{
				Type             = InventoryObjectsTypesEnum.TEST_OBJECT,
				ToChange         = 0,
				ResultCount      = 0,
				ResultPacksCount = 0,
				ResultChange     = false
			},
			new InventoryTestData
			{
				Type             = InventoryObjectsTypesEnum.TEST_OBJECT,
				ToChange         = 1,
				ResultCount      = 0,
				ResultPacksCount = 0,
				ResultChange     = false
			},
			new InventoryTestData
			{
				Type             = InventoryObjectsTypesEnum.TEST_OBJECT,
				InitCount        = 0,
				ToChange         = 51,
				ResultCount      = 0,
				ResultPacksCount = 0,
				ResultChange     = false
			},
		};

		private static InventoryTestData[] _removeNotConfiguredTestData =
		{
			new InventoryTestData
			{
				Type             = InventoryObjectsTypesEnum.TEST_OBJECT,
				InitCount        = 100,
				ResultCount      = 0,
				ResultPacksCount = 0,
				ToChange         = -1,
				ResultChange     = false
			},
			new InventoryTestData
			{
				Type             = InventoryObjectsTypesEnum.TEST_OBJECT,
				InitCount        = 100,
				ResultCount      = 0,
				ResultPacksCount = 0,
				ToChange         = 0,
				ResultChange     = false
			},
			new InventoryTestData
			{
				Type             = InventoryObjectsTypesEnum.TEST_OBJECT,
				InitCount        = 100,
				ResultCount      = 0,
				ResultPacksCount = 0,
				ToChange         = 1,
				ResultChange     = false
			},
			new InventoryTestData
			{
				Type             = InventoryObjectsTypesEnum.TEST_OBJECT,
				InitCount        = 100,
				ResultCount      = 0,
				ResultPacksCount = 0,
				ToChange         = 100,
				ResultChange     = false
			},
			new InventoryTestData
			{
				Type             = InventoryObjectsTypesEnum.TEST_OBJECT,
				InitCount        = 100,
				ResultCount      = 0,
				ResultPacksCount = 0,
				ToChange         = 101,
				ResultChange     = false
			},
		};

		private static InventoryLimitTestData[] _limitTestData =
		{
			new InventoryLimitTestData
			{
				Type         = InventoryObjectsTypesEnum.TEST_OBJECT,
				ToChange     = -1,
				ResultChange = false,
				ResultEdge   = 0
			},
			new InventoryLimitTestData
			{
				Type         = InventoryObjectsTypesEnum.TEST_OBJECT,
				ToChange     = 0,
				ResultChange = true,
				ResultEdge   = 0
			},
			new InventoryLimitTestData
			{
				Type         = InventoryObjectsTypesEnum.TEST_OBJECT,
				ToChange     = 1,
				ResultChange = true,
				ResultEdge   = 0
			},
			new InventoryLimitTestData
			{
				Type         = InventoryObjectsTypesEnum.TEST_OBJECT,
				ToChange     = 50,
				ResultChange = true,
				ResultEdge   = 0
			},
			new InventoryLimitTestData
			{
				Type         = InventoryObjectsTypesEnum.TEST_OBJECT,
				ToChange     = 51,
				ResultChange = true,
				ResultEdge   = 1
			},
		};

		private static InventoryLimitTestData[] _limitPackTestData =
		{
			new InventoryLimitTestData
			{
				Type         = InventoryObjectsTypesEnum.TEST_OBJECT,
				InitCount    = 0,
				ToChange     = 1,
				ResultChange = true,
				ResultEdge   = 0
			},
			new InventoryLimitTestData
			{
				Type         = InventoryObjectsTypesEnum.TEST_OBJECT,
				InitCount    = 0,
				ToChange     = 50,
				ResultChange = true,
				ResultEdge   = 0
			},
			new InventoryLimitTestData
			{
				Type         = InventoryObjectsTypesEnum.TEST_OBJECT,
				InitCount    = 50,
				ToChange     = 1,
				ResultChange = true,
				ResultEdge   = 1
			},
			new InventoryLimitTestData
			{
				Type         = InventoryObjectsTypesEnum.TEST_OBJECT,
				InitCount    = 50,
				ToChange     = 50,
				ResultChange = true,
				ResultEdge   = 50
			},
		};

		private static InventoryLimitTestData[] _limitFullPackTestData =
		{
			new InventoryLimitTestData
			{
				Type         = InventoryObjectsTypesEnum.TEST_OBJECT,
				InitCount    = 0,
				ToChange     = 51,
				ResultChange = true,
				ResultEdge   = 1
			},
			new InventoryLimitTestData
			{
				Type         = InventoryObjectsTypesEnum.TEST_OBJECT,
				InitCount    = 50,
				ToChange     = 51,
				ResultChange = true,
				ResultEdge   = 51
			},
		};

		#endregion

		private IInventory                   _inventory;
		private InventoryPacksModelsSettings _inventoryPacksModelsSettings;

		public override void Setup()
		{
			base.Setup();
			_inventoryPacksModelsSettings = new InventoryPacksModelsSettings();
			_inventoryPacksModelsSettings.Inventories.Add(InventoryTypesModel.GetUnlimitedTestModel());
			_inventoryPacksModelsSettings.Inventories.Add(InventoryTypesModel.GetTestModel());
			_inventoryPacksModelsSettings.Prepare();
			Container.Bind<InventoryPacksModelsSettings>().FromInstance(_inventoryPacksModelsSettings).AsSingle();

			_inventory = new Inventories.Inventory();
			Container.Inject(_inventory);
		}

		public override void Teardown()
		{
			base.Teardown();
			Factory.Clear();
			Container.Unbind<InventoryPacksModelsSettings>();
			_inventory.Dispose();
			_inventory                    = null;
			_inventoryPacksModelsSettings = null;
		}

		private void AddMoqConfig()
		{
			_inventoryPacksModelsSettings.Models.Add(InventoryPackModel.GetTestModel());
		}

		private void CheckResults(InventoryObjectsTypesEnum type,
		                          bool                      methodResult,
		                          bool                      methodResultExpected,
		                          int                       resultCount,
		                          int                       resultPacksCount)
		{
			Assert.AreEqual(methodResultExpected, methodResult);
			Assert.AreEqual(resultCount,          _inventory.ItemsCount(type));
			Assert.AreEqual(resultCount,
			                _inventory.GetPacks().Where(pack => pack.Model.Type == type)
			                          .Sum(pack => pack.Size.Value));
			Assert.AreEqual(resultCount,      _inventory.GetPacks(type).Count.Value);
			Assert.AreEqual(resultPacksCount, _inventory.GetPacks().Count(pack => pack.Model.Type == type));
			Assert.AreEqual(resultPacksCount, _inventory.GetPacks(type).Packs.Count());
		}

		private void CheckNullResults(InventoryObjectsTypesEnum type,
		                              bool                      methodResult)
		{
			Assert.AreEqual(false, methodResult);
			Assert.AreEqual(0,     _inventory.ItemsCount(type));
			Assert.AreEqual(0,
			                _inventory.GetPacks().Where(pack => pack.Model.Type == type)
			                          .Sum(pack => pack.Size.Value));
			Assert.AreEqual(0, _inventory.GetPacks().Count(pack => pack.Model.Type == type));
			Assert.IsNull(_inventory.GetPacks(type));
		}

		private void CheckResults(InventoryObjectsTypesEnum type,
		                          bool                      methodResult,
		                          InventoryTestData         data)
		{
			CheckResults(type, methodResult, data.ResultChange, data.ResultCount, data.ResultPacksCount);
		}

		private void CheckNullResults(InventoryObjectsTypesEnum type,
		                              bool                      methodResult,
		                              InventoryTestData         data)
		{
			CheckNullResults(type, methodResult);
		}

		[Test]
		public void AddToInventoryConfiguredItemTest([ValueSource("_addItemTestDatas")] InventoryTestData data)
		{
			AddMoqConfig();
			var resultType = data.Type;
			var addResult  = _inventory.AddItems(data.Type, data.ToChange, out var edge);

			CheckResults(resultType, addResult, data);
		}

		[Test]
		public void RemoveFromInventoryConfiguredItemTest([ValueSource("_removeTestData")] InventoryTestData data)
		{
			AddMoqConfig();
			var resultType = data.Type;
			_inventory.AddItems(resultType, data.InitCount, out var edge);
			var removeResult = _inventory.RemoveItem(resultType, data.ToChange);

			CheckResults(resultType, removeResult, data);
		}

		[Test]
		public void AddToInventoryNotConfiguredItemTest(
			[ValueSource("_addItemNotConfiguredTestDatas")] InventoryTestData data)
		{
			var resultType = data.Type;
			var addResult  = _inventory.AddItems(data.Type, data.ToChange, out var edge);

			CheckNullResults(resultType, addResult, data);
		}

		[Test]
		public void RemoveFromInventoryNotConfiguredItemTest(
			[ValueSource("_removeNotConfiguredTestData")] InventoryTestData data)
		{
			var resultType = data.Type;
			_inventory.AddItems(resultType, data.InitCount, out var edge);
			var removeResult = _inventory.RemoveItem(resultType, data.ToChange);

			CheckNullResults(resultType, removeResult, data);
		}

		[Test]
		public void AddToInventoryConfiguredPackTest()
		{
			AddMoqConfig();
			const int                       resultCount               = 10;
			const int                       resultPacksCount          = 1;
			const InventoryObjectsTypesEnum InventoryObjectsTypesEnum = InventoryObjectsTypesEnum.TEST_OBJECT;
			var                             addPack                   = Factory.GetFactoryItem<InventoryPack>();
			addPack.Initialize(InventoryPackModel.GetTestModel(), resultCount);
			var addResult = _inventory.AddItems(addPack, out var edge);

			CheckResults(InventoryObjectsTypesEnum, addResult, true, resultCount, resultPacksCount);
		}

		[Test]
		public void AddToInventoryNotConfiguredPackTest()
		{
			const int                       resultCount               = 10;
			const InventoryObjectsTypesEnum InventoryObjectsTypesEnum = InventoryObjectsTypesEnum.TEST_OBJECT;
			var                             addPack                   = Factory.GetFactoryItem<InventoryPack>();
			addPack.Initialize(InventoryPackModel.GetTestModel(), resultCount);
			var addResult = _inventory.AddItems(addPack, out var edge);

			CheckNullResults(InventoryObjectsTypesEnum, addResult);
		}

		[Test]
		public void AddToInventoryConfigureFullPackTest()
		{
			AddMoqConfig();
			const int                       resultCount               = 10;
			const int                       resultPacksCount          = 1;
			const InventoryObjectsTypesEnum InventoryObjectsTypesEnum = InventoryObjectsTypesEnum.TEST_OBJECT;

			var addPack = Factory.GetFactoryItem<FullInventoryPack>();
			addPack.Initialize(null, InventoryPackModel.GetTestModel());
			addPack.Add(resultCount);
			var addResult = _inventory.AddItems(addPack, out int edge);

			CheckResults(InventoryObjectsTypesEnum, addResult, true, resultCount, resultPacksCount);
		}

		[Test]
		public void AddToInventoryNotConfiguredFullPackTest()
		{
			const int                       resultCount               = 10;
			const InventoryObjectsTypesEnum InventoryObjectsTypesEnum = InventoryObjectsTypesEnum.TEST_OBJECT;

			var addPack = Factory.GetFactoryItem<FullInventoryPack>();
			addPack.Initialize(null, InventoryPackModel.GetTestModel());
			addPack.Add(resultCount);
			var addResult = _inventory.AddItems(addPack, out int edge);

			CheckNullResults(InventoryObjectsTypesEnum, addResult);
		}

		[Test]
		public void RemoveFromInventoryConfiguredPackTest()
		{
			AddMoqConfig();
			const int                       resultCount               = 10;
			const InventoryObjectsTypesEnum InventoryObjectsTypesEnum = InventoryObjectsTypesEnum.TEST_OBJECT;
			var                             addPack                   = Factory.GetFactoryItem<InventoryPack>();
			addPack.Initialize(InventoryPackModel.GetTestModel(), resultCount);
			_inventory.AddItems(addPack, out var edge);

			Assert.AreEqual(resultCount, _inventory.ItemsCount(InventoryObjectsTypesEnum));

			var removeResult =
				_inventory.RemoveItem(_inventory.GetPacks()
				                                .First(pack => pack.Model.Type == InventoryObjectsTypesEnum));

			CheckResults(InventoryObjectsTypesEnum, removeResult, true, 0, 0);
		}

		[Test]
		public void RemoveFromEmptyInventoryConfiguredPackTest()
		{
			AddMoqConfig();
			const int                       resultCount               = 10;
			const InventoryObjectsTypesEnum InventoryObjectsTypesEnum = InventoryObjectsTypesEnum.TEST_OBJECT;
			var                             addPack                   = Factory.GetFactoryItem<InventoryPack>();
			addPack.Initialize(InventoryPackModel.GetTestModel(), resultCount);

			var removeResult = _inventory.RemoveItem(addPack);

			CheckResults(InventoryObjectsTypesEnum, removeResult, true, 0, 0);
		}

		[Test]
		public void RemoveFromInventoryNotInventoryConfiguredPackTest()
		{
			AddMoqConfig();
			const int                       resultCount               = 10;
			const InventoryObjectsTypesEnum InventoryObjectsTypesEnum = InventoryObjectsTypesEnum.TEST_OBJECT;
			var                             addPack                   = Factory.GetFactoryItem<InventoryPack>();
			addPack.Initialize(InventoryPackModel.GetTestModel(), resultCount);
			_inventory.AddItems(addPack, out var edge);

			Assert.AreEqual(resultCount, _inventory.ItemsCount(InventoryObjectsTypesEnum));

			addPack = Factory.GetFactoryItem<InventoryPack>();
			addPack.Initialize(InventoryPackModel.GetTestModel(), resultCount);
			var removeResult = _inventory.RemoveItem(addPack);

			CheckResults(InventoryObjectsTypesEnum, removeResult, true, 10, 1);
		}

		[Test]
		public void RemoveFromInventoryNotInventoryConfiguredFullPackTest()
		{
			AddMoqConfig();
			const int                       resultCount               = 10;
			const InventoryObjectsTypesEnum InventoryObjectsTypesEnum = InventoryObjectsTypesEnum.TEST_OBJECT;
			var                             addPack                   = Factory.GetFactoryItem<InventoryPack>();
			addPack.Initialize(InventoryPackModel.GetTestModel(), resultCount);
			_inventory.AddItems(addPack, out var edge);

			Assert.AreEqual(resultCount, _inventory.ItemsCount(InventoryObjectsTypesEnum));

			var newPack = Factory.GetFactoryItem<FullInventoryPack>();
			newPack.Initialize(null, InventoryPackModel.GetTestModel());
			newPack.Add(resultCount);
			var removeResult = _inventory.RemoveItem(newPack);

			CheckResults(InventoryObjectsTypesEnum, removeResult, false, 10, 1);
		}

		[Test]
		public void RemoveFromInventoryConfiguredFullPackTest()
		{
			AddMoqConfig();
			const int                       resultCount               = 10;
			const InventoryObjectsTypesEnum InventoryObjectsTypesEnum = InventoryObjectsTypesEnum.TEST_OBJECT;
			var                             addPack                   = Factory.GetFactoryItem<InventoryPack>();
			addPack.Initialize(InventoryPackModel.GetTestModel(), resultCount);
			_inventory.AddItems(addPack, out var edge);

			Assert.AreEqual(resultCount, _inventory.ItemsCount(InventoryObjectsTypesEnum));

			var removeResult = _inventory.RemoveItem(_inventory.GetPacks(InventoryObjectsTypesEnum));

			CheckResults(InventoryObjectsTypesEnum, removeResult, true, 0, 0);
		}

		[Test]
		public void RemoveFromEmptyInventoryNotConfiguredPackTest()
		{
			const int                       resultCount               = 10;
			const InventoryObjectsTypesEnum InventoryObjectsTypesEnum = InventoryObjectsTypesEnum.TEST_OBJECT;

			var removePack = Factory.GetFactoryItem<InventoryPack>();
			removePack.Initialize(InventoryPackModel.GetTestModel(), resultCount);

			var removeResult = _inventory.RemoveItem(removePack);

			CheckNullResults(InventoryObjectsTypesEnum, removeResult);
		}

		[Test]
		public void RemoveFromInventoryNotInventoryNotConfiguredPackTest()
		{
			AddMoqConfig();
			const int                       resultCount               = 10;
			const InventoryObjectsTypesEnum InventoryObjectsTypesEnum = InventoryObjectsTypesEnum.TEST_OBJECT;
			const InventoryObjectsTypesEnum nothingTypesEnum          = InventoryObjectsTypesEnum.NOTHING;
			var                             addPack                   = Factory.GetFactoryItem<InventoryPack>();
			addPack.Initialize(InventoryPackModel.GetTestModel(), resultCount);
			_inventory.AddItems(addPack, out var edge);

			Assert.AreEqual(resultCount, _inventory.ItemsCount(InventoryObjectsTypesEnum));

			addPack = Factory.GetFactoryItem<InventoryPack>();
			addPack.Initialize(InventoryPackModel.GetNothingTestModel(), resultCount);
			var removeResult = _inventory.RemoveItem(addPack);

			CheckResults(InventoryObjectsTypesEnum, true, true, 10, 1);
			CheckNullResults(nothingTypesEnum, removeResult);
		}

		[Test]
		public void RemoveFromInventoryNotInventoryNotConfiguredFullPackTest()
		{
			AddMoqConfig();
			const int                       resultCount               = 10;
			const InventoryObjectsTypesEnum InventoryObjectsTypesEnum = InventoryObjectsTypesEnum.TEST_OBJECT;
			const InventoryObjectsTypesEnum nothingTypesEnum          = InventoryObjectsTypesEnum.NOTHING;
			var                             addPack                   = Factory.GetFactoryItem<InventoryPack>();
			addPack.Initialize(InventoryPackModel.GetTestModel(), resultCount);
			_inventory.AddItems(addPack, out var edge);

			Assert.AreEqual(resultCount, _inventory.ItemsCount(InventoryObjectsTypesEnum));

			var newPack = Factory.GetFactoryItem<FullInventoryPack>();
			newPack.Initialize(null, InventoryPackModel.GetNothingTestModel());
			newPack.Add(resultCount);
			var removeResult = _inventory.RemoveItem(newPack);

			CheckResults(InventoryObjectsTypesEnum, true, true, 10, 1);
			CheckNullResults(nothingTypesEnum, removeResult);
		}

		[Test]
		public void RemoveFromInventoryNotConfiguredFullPackTest()
		{
			const InventoryObjectsTypesEnum InventoryObjectsTypesEnum = InventoryObjectsTypesEnum.TEST_OBJECT;

			var newPack = Factory.GetFactoryItem<FullInventoryPack>();
			newPack.Initialize(null, InventoryPackModel.GetTestModel());
			newPack.Add(10);
			var removeResult = _inventory.RemoveItem(newPack);

			CheckNullResults(InventoryObjectsTypesEnum, removeResult);
		}

		[Test]
		public void LimitInventoryTests([ValueSource("_limitTestData")] InventoryLimitTestData data)
		{
			AddMoqConfig();
			_inventory.Initialize(InventoryTypesEnum.TEST);

			var result = _inventory.AddItems(data.Type, data.ToChange, out var edge);
			Assert.AreEqual(data.ResultChange, result);
			Assert.AreEqual(data.ResultEdge,   edge);
		}

		[Test]
		public void LimitPackInventoryTests([ValueSource("_limitPackTestData")] InventoryLimitTestData data)
		{
			AddMoqConfig();
			_inventory.Initialize(InventoryTypesEnum.TEST);
			var inventoryPack = Factory.GetFactoryItem<InventoryPack>();
			inventoryPack.Initialize(InventoryPackModel.GetTestModel(), data.InitCount);
			_inventory.AddItems(inventoryPack, out var edge);

			inventoryPack = Factory.GetFactoryItem<InventoryPack>();
			inventoryPack.Initialize(InventoryPackModel.GetTestModel(), data.ToChange);
			var result = _inventory.AddItems(inventoryPack, out edge);
			Assert.AreEqual(data.ResultChange, result);
			Assert.AreEqual(data.ResultEdge,   edge);
		}

		[Test]
		public void LimitFullPackInventoryTests([ValueSource("_limitFullPackTestData")] InventoryLimitTestData data)
		{
			AddMoqConfig();
			_inventory.Initialize(InventoryTypesEnum.TEST);
			var inventoryPack = Factory.GetFactoryItem<FullInventoryPack>();
			inventoryPack.Initialize(null, InventoryPackModel.GetTestModel());
			inventoryPack.Add(data.InitCount);
			_inventory.AddItems(inventoryPack, out var edge);

			inventoryPack = Factory.GetFactoryItem<FullInventoryPack>();
			inventoryPack.Initialize(null, InventoryPackModel.GetTestModel());
			inventoryPack.Add(data.ToChange);
			var result = _inventory.AddItems(inventoryPack, out edge);
			Assert.AreEqual(data.ResultChange, result);
			Assert.AreEqual(data.ResultEdge,   edge);
		}
	}
}