using System;
using System.Linq;
using Factories;
using Inventory;
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
			public InventoryTypesEnum Type;
			public bool               ResultChange;
			public int                ToChange;
			public int                InitCount;
			public int                ResultCount;
			public int                ResultPacksCount;
		}

		#endregion

		#region Test datas

		private static InventoryTestData[] _addItemTestDatas =
		{
			new InventoryTestData
			{
				Type             = InventoryTypesEnum.TEST_OBJECT,
				ToChange         = -1,
				ResultCount      = 0,
				ResultPacksCount = 0,
				ResultChange     = true
			},
			new InventoryTestData
			{
				Type             = InventoryTypesEnum.TEST_OBJECT,
				ToChange         = 0,
				ResultCount      = 0,
				ResultPacksCount = 0,
				ResultChange     = true
			},
			new InventoryTestData
			{
				Type             = InventoryTypesEnum.TEST_OBJECT,
				ToChange         = 1,
				ResultCount      = 1,
				ResultPacksCount = 1,
				ResultChange     = true
			},
			new InventoryTestData
			{
				Type             = InventoryTypesEnum.TEST_OBJECT,
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
				Type             = InventoryTypesEnum.TEST_OBJECT,
				InitCount        = 100,
				ResultCount      = 100,
				ResultPacksCount = 2,
				ToChange         = -1,
				ResultChange     = true
			},
			new InventoryTestData
			{
				Type             = InventoryTypesEnum.TEST_OBJECT,
				InitCount        = 100,
				ResultCount      = 100,
				ResultPacksCount = 2,
				ToChange         = 0,
				ResultChange     = true
			},
			new InventoryTestData
			{
				Type             = InventoryTypesEnum.TEST_OBJECT,
				InitCount        = 100,
				ResultCount      = 99,
				ResultPacksCount = 2,
				ToChange         = 1,
				ResultChange     = true
			},
			new InventoryTestData
			{
				Type             = InventoryTypesEnum.TEST_OBJECT,
				InitCount        = 100,
				ResultCount      = 0,
				ResultPacksCount = 0,
				ToChange         = 100,
				ResultChange     = true
			},
			new InventoryTestData
			{
				Type             = InventoryTypesEnum.TEST_OBJECT,
				InitCount        = 100,
				ResultCount      = 0,
				ResultPacksCount = 0,
				ToChange         = 101,
				ResultChange     = false
			},
			new InventoryTestData
			{
				Type             = InventoryTypesEnum.TEST_OBJECT,
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
				Type             = InventoryTypesEnum.TEST_OBJECT,
				ToChange         = -1,
				ResultCount      = 0,
				ResultPacksCount = 0,
				ResultChange     = false
			},
			new InventoryTestData
			{
				Type             = InventoryTypesEnum.TEST_OBJECT,
				ToChange         = 0,
				ResultCount      = 0,
				ResultPacksCount = 0,
				ResultChange     = false
			},
			new InventoryTestData
			{
				Type             = InventoryTypesEnum.TEST_OBJECT,
				ToChange         = 1,
				ResultCount      = 0,
				ResultPacksCount = 0,
				ResultChange     = false
			},
			new InventoryTestData
			{
				Type             = InventoryTypesEnum.TEST_OBJECT,
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
				Type             = InventoryTypesEnum.TEST_OBJECT,
				InitCount        = 100,
				ResultCount      = 0,
				ResultPacksCount = 0,
				ToChange         = -1,
				ResultChange     = false
			},
			new InventoryTestData
			{
				Type             = InventoryTypesEnum.TEST_OBJECT,
				InitCount        = 100,
				ResultCount      = 0,
				ResultPacksCount = 0,
				ToChange         = 0,
				ResultChange     = false
			},
			new InventoryTestData
			{
				Type             = InventoryTypesEnum.TEST_OBJECT,
				InitCount        = 100,
				ResultCount      = 0,
				ResultPacksCount = 0,
				ToChange         = 1,
				ResultChange     = false
			},
			new InventoryTestData
			{
				Type             = InventoryTypesEnum.TEST_OBJECT,
				InitCount        = 100,
				ResultCount      = 0,
				ResultPacksCount = 0,
				ToChange         = 100,
				ResultChange     = false
			},
			new InventoryTestData
			{
				Type             = InventoryTypesEnum.TEST_OBJECT,
				InitCount        = 100,
				ResultCount      = 0,
				ResultPacksCount = 0,
				ToChange         = 101,
				ResultChange     = false
			},
		};

		#endregion

		private IInventory                   _inventory;
		private InventoryPacksModelsSettings _inventoryPacksModelsSettings;

		public override void Setup()
		{
			base.Setup();
			_inventoryPacksModelsSettings = new InventoryPacksModelsSettings();
			Container.Bind<InventoryPacksModelsSettings>().FromInstance(_inventoryPacksModelsSettings).AsSingle();

			_inventory = new global::Inventory.Inventory();
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

		private void CheckResults(InventoryTypesEnum type,
		                          bool               methodResult,
		                          bool               methodResultExpected,
		                          int                resultCount,
		                          int                resultPacksCount)
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

		private void CheckNullResults(InventoryTypesEnum type,
		                              bool               methodResult)
		{
			Assert.AreEqual(false, methodResult);
			Assert.AreEqual(0,     _inventory.ItemsCount(type));
			Assert.AreEqual(0,
			                _inventory.GetPacks().Where(pack => pack.Model.Type == type)
			                          .Sum(pack => pack.Size.Value));
			Assert.AreEqual(0, _inventory.GetPacks().Count(pack => pack.Model.Type == type));
			Assert.IsNull(_inventory.GetPacks(type));
		}

		private void CheckResults(InventoryTypesEnum type,
		                          bool               methodResult,
		                          InventoryTestData  data)
		{
			CheckResults(type, methodResult, data.ResultChange, data.ResultCount, data.ResultPacksCount);
		}

		private void CheckNullResults(InventoryTypesEnum type,
		                              bool               methodResult,
		                              InventoryTestData  data)
		{
			CheckNullResults(type, methodResult);
		}

		[Test]
		public void AddToInventoryConfiguredItemTest([ValueSource("_addItemTestDatas")] InventoryTestData data)
		{
			AddMoqConfig();
			var resultType = data.Type;
			var addResult  = _inventory.AddItems(data.Type, data.ToChange);

			CheckResults(resultType, addResult, data);
		}

		[Test]
		public void RemoveFromInventoryConfiguredItemTest([ValueSource("_removeTestData")] InventoryTestData data)
		{
			AddMoqConfig();
			var resultType = data.Type;
			_inventory.AddItems(resultType, data.InitCount);
			var removeResult = _inventory.RemoveItem(resultType, data.ToChange);

			CheckResults(resultType, removeResult, data);
		}

		[Test]
		public void AddToInventoryNotConfiguredItemTest(
			[ValueSource("_addItemNotConfiguredTestDatas")] InventoryTestData data)
		{
			var resultType = data.Type;
			var addResult  = _inventory.AddItems(data.Type, data.ToChange);

			CheckNullResults(resultType, addResult, data);
		}

		[Test]
		public void RemoveFromInventoryNotConfiguredItemTest(
			[ValueSource("_removeNotConfiguredTestData")] InventoryTestData data)
		{
			var resultType = data.Type;
			_inventory.AddItems(resultType, data.InitCount);
			var removeResult = _inventory.RemoveItem(resultType, data.ToChange);

			CheckNullResults(resultType, removeResult, data);
		}

		[Test]
		public void AddToInventoryConfiguredPackTest()
		{
			AddMoqConfig();
			const int                resultCount        = 10;
			const int                resultPacksCount   = 1;
			const InventoryTypesEnum inventoryTypesEnum = InventoryTypesEnum.TEST_OBJECT;
			var                      addPack            = Factory.GetFactoryItem<InventoryPack>();
			addPack.Initialize(InventoryPackModel.GetTestModel(), resultCount);
			var addResult = _inventory.AddItems(addPack);

			CheckResults(inventoryTypesEnum, addResult, true, resultCount, resultPacksCount);
		}

		[Test]
		public void AddToInventoryNotConfiguredPackTest()
		{
			const int                resultCount        = 10;
			const InventoryTypesEnum inventoryTypesEnum = InventoryTypesEnum.TEST_OBJECT;
			var                      addPack            = Factory.GetFactoryItem<InventoryPack>();
			addPack.Initialize(InventoryPackModel.GetTestModel(), resultCount);
			var addResult = _inventory.AddItems(addPack);

			CheckNullResults(inventoryTypesEnum, addResult);
		}

		[Test]
		public void AddToInventoryConfigureFullPackTest()
		{
			AddMoqConfig();
			const int                resultCount        = 10;
			const int                resultPacksCount   = 1;
			const InventoryTypesEnum inventoryTypesEnum = InventoryTypesEnum.TEST_OBJECT;

			var addPack = Factory.GetFactoryItem<FullInventoryPack>();
			addPack.Initialize(null, InventoryPackModel.GetTestModel());
			addPack.Add(resultCount);
			var addResult = _inventory.AddItems(addPack);

			CheckResults(inventoryTypesEnum, addResult, true, resultCount, resultPacksCount);
		}

		[Test]
		public void AddToInventoryNotConfiguredFullPackTest()
		{
			const int                resultCount        = 10;
			const InventoryTypesEnum inventoryTypesEnum = InventoryTypesEnum.TEST_OBJECT;

			var addPack = Factory.GetFactoryItem<FullInventoryPack>();
			addPack.Initialize(null, InventoryPackModel.GetTestModel());
			addPack.Add(resultCount);
			var addResult = _inventory.AddItems(addPack);

			CheckNullResults(inventoryTypesEnum, addResult);
		}

		[Test]
		public void RemoveFromInventoryConfiguredPackTest()
		{
			AddMoqConfig();
			const int                resultCount        = 10;
			const InventoryTypesEnum inventoryTypesEnum = InventoryTypesEnum.TEST_OBJECT;
			var                      addPack            = Factory.GetFactoryItem<InventoryPack>();
			addPack.Initialize(InventoryPackModel.GetTestModel(), resultCount);
			_inventory.AddItems(addPack);

			Assert.AreEqual(resultCount, _inventory.ItemsCount(inventoryTypesEnum));

			var removeResult =
				_inventory.RemoveItem(_inventory.GetPacks().First(pack => pack.Model.Type == inventoryTypesEnum));

			CheckResults(inventoryTypesEnum, removeResult, true, 0, 0);
		}

		[Test]
		public void RemoveFromEmptyInventoryConfiguredPackTest()
		{
			AddMoqConfig();
			const int                resultCount        = 10;
			const InventoryTypesEnum inventoryTypesEnum = InventoryTypesEnum.TEST_OBJECT;
			var                      addPack            = Factory.GetFactoryItem<InventoryPack>();
			addPack.Initialize(InventoryPackModel.GetTestModel(), resultCount);

			var removeResult = _inventory.RemoveItem(addPack);

			CheckResults(inventoryTypesEnum, removeResult, true, 0, 0);
		}

		[Test]
		public void RemoveFromInventoryNotInventoryConfiguredPackTest()
		{
			AddMoqConfig();
			const int                resultCount        = 10;
			const InventoryTypesEnum inventoryTypesEnum = InventoryTypesEnum.TEST_OBJECT;
			var                      addPack            = Factory.GetFactoryItem<InventoryPack>();
			addPack.Initialize(InventoryPackModel.GetTestModel(), resultCount);
			_inventory.AddItems(addPack);

			Assert.AreEqual(resultCount, _inventory.ItemsCount(inventoryTypesEnum));

			addPack = Factory.GetFactoryItem<InventoryPack>();
			addPack.Initialize(InventoryPackModel.GetTestModel(), resultCount);
			var removeResult = _inventory.RemoveItem(addPack);

			CheckResults(inventoryTypesEnum, removeResult, true, 10, 1);
		}

		[Test]
		public void RemoveFromInventoryNotInventoryConfiguredFullPackTest()
		{
			AddMoqConfig();
			const int                resultCount        = 10;
			const InventoryTypesEnum inventoryTypesEnum = InventoryTypesEnum.TEST_OBJECT;
			var                      addPack            = Factory.GetFactoryItem<InventoryPack>();
			addPack.Initialize(InventoryPackModel.GetTestModel(), resultCount);
			_inventory.AddItems(addPack);

			Assert.AreEqual(resultCount, _inventory.ItemsCount(inventoryTypesEnum));

			var newPack = Factory.GetFactoryItem<FullInventoryPack>();
			newPack.Initialize(null, InventoryPackModel.GetTestModel());
			newPack.Add(resultCount);
			var removeResult = _inventory.RemoveItem(newPack);

			CheckResults(inventoryTypesEnum, removeResult, false, 10, 1);
		}

		[Test]
		public void RemoveFromInventoryConfiguredFullPackTest()
		{
			AddMoqConfig();
			const int                resultCount        = 10;
			const InventoryTypesEnum inventoryTypesEnum = InventoryTypesEnum.TEST_OBJECT;
			var                      addPack            = Factory.GetFactoryItem<InventoryPack>();
			addPack.Initialize(InventoryPackModel.GetTestModel(), resultCount);
			_inventory.AddItems(addPack);

			Assert.AreEqual(resultCount, _inventory.ItemsCount(inventoryTypesEnum));

			var removeResult = _inventory.RemoveItem(_inventory.GetPacks(inventoryTypesEnum));

			CheckResults(inventoryTypesEnum, removeResult, true, 0, 0);
		}

		[Test]
		public void RemoveFromEmptyInventoryNotConfiguredPackTest()
		{
			const int                resultCount        = 10;
			const InventoryTypesEnum inventoryTypesEnum = InventoryTypesEnum.TEST_OBJECT;

			var removePack = Factory.GetFactoryItem<InventoryPack>();
			removePack.Initialize(InventoryPackModel.GetTestModel(), resultCount);

			var removeResult = _inventory.RemoveItem(removePack);

			CheckNullResults(inventoryTypesEnum, removeResult);
		}

		[Test]
		public void RemoveFromInventoryNotInventoryNotConfiguredPackTest()
		{
			AddMoqConfig();
			const int                resultCount        = 10;
			const InventoryTypesEnum inventoryTypesEnum = InventoryTypesEnum.TEST_OBJECT;
			const InventoryTypesEnum nothingTypesEnum   = InventoryTypesEnum.NOTHING;
			var                      addPack            = Factory.GetFactoryItem<InventoryPack>();
			addPack.Initialize(InventoryPackModel.GetTestModel(), resultCount);
			_inventory.AddItems(addPack);

			Assert.AreEqual(resultCount, _inventory.ItemsCount(inventoryTypesEnum));

			addPack = Factory.GetFactoryItem<InventoryPack>();
			addPack.Initialize(InventoryPackModel.GetNothingTestModel(), resultCount);
			var removeResult = _inventory.RemoveItem(addPack);

			CheckResults(inventoryTypesEnum, true, true, 10, 1);
			CheckNullResults(nothingTypesEnum, removeResult);
		}

		[Test]
		public void RemoveFromInventoryNotInventoryNotConfiguredFullPackTest()
		{
			AddMoqConfig();
			const int                resultCount        = 10;
			const InventoryTypesEnum inventoryTypesEnum = InventoryTypesEnum.TEST_OBJECT;
			const InventoryTypesEnum nothingTypesEnum   = InventoryTypesEnum.NOTHING;
			var                      addPack            = Factory.GetFactoryItem<InventoryPack>();
			addPack.Initialize(InventoryPackModel.GetTestModel(), resultCount);
			_inventory.AddItems(addPack);

			Assert.AreEqual(resultCount, _inventory.ItemsCount(inventoryTypesEnum));

			var newPack = Factory.GetFactoryItem<FullInventoryPack>();
			newPack.Initialize(null, InventoryPackModel.GetNothingTestModel());
			newPack.Add(resultCount);
			var removeResult = _inventory.RemoveItem(newPack);

			CheckResults(inventoryTypesEnum, true, true, 10, 1);
			CheckNullResults(nothingTypesEnum, removeResult);
		}

		[Test]
		public void RemoveFromInventoryNotConfiguredFullPackTest()
		{
			const InventoryTypesEnum inventoryTypesEnum = InventoryTypesEnum.TEST_OBJECT;

			var newPack = Factory.GetFactoryItem<FullInventoryPack>();
			newPack.Initialize(null, InventoryPackModel.GetTestModel());
			newPack.Add(10);
			var removeResult = _inventory.RemoveItem(newPack);

			CheckNullResults(inventoryTypesEnum, removeResult);
		}

		/*[Test]
		public void AddToInventoryNotConfigItemTest()
		{
			Assert.False(_inventory.AddItems(InventoryTypesEnum.TEST_OBJECT));
		}
	
		[Test]
		public void AddToInventoryNotConfigItemPackTest()
		{
			var inventoryPack = Factory.GetFactoryItem<InventoryPack>();
			inventoryPack.Initialize(InventoryPackModel.GetTestModel(), 500);
			Assert.False(_inventory.AddItems(inventoryPack));
		}
	
		[Test]
		public void AddToInventoryNotConfigItemFullPackTest()
		{
			var fullInventoryPack = Factory.GetFactoryItem<FullInventoryPack>();
			fullInventoryPack.Initialize(null, InventoryPackModel.GetTestModel());
			fullInventoryPack.Add(500);
			Assert.False(_inventory.AddItems(fullInventoryPack));
		}
	
		[Test]
		public void RemoveFromInventoryNotConfigItemTest()
		{
			Assert.False(_inventory.RemoveItem(InventoryTypesEnum.TEST_OBJECT));
		}
	
		[Test]
		public void RemoveFromInventoryNotConfigItemPackTest()
		{
			var inventoryPack = Factory.GetFactoryItem<InventoryPack>();
			inventoryPack.Initialize(InventoryPackModel.GetTestModel(), 500);
			Assert.False(_inventory.RemoveItem(inventoryPack));
		}
	
		[Test]
		public void RemoveFromInventoryNotConfigItemFullPackTest()
		{
			var fullInventoryPack = Factory.GetFactoryItem<FullInventoryPack>();
			fullInventoryPack.Initialize(null, InventoryPackModel.GetTestModel());
			fullInventoryPack.Add(500);
			Assert.False(_inventory.RemoveItem(fullInventoryPack));
		}
	
		[Test]
		public void GetFromInventoryNotConfigItemsCountTest()
		{
			Assert.Zero(_inventory.ItemsCount(InventoryTypesEnum.TEST_OBJECT));
		}
	
		[Test]
		public void AddToInventoryConfigItemTest()
		{
			AddMoqConfig();
			Assert.True(_inventory.AddItems(InventoryTypesEnum.TEST_OBJECT));
		}
	
		[Test]
		public void RemoveFromInventoryConfigItemTest()
		{
			AddMoqConfig();
			_inventory.AddItems(InventoryTypesEnum.TEST_OBJECT);
			Assert.True(_inventory.RemoveItem(InventoryTypesEnum.TEST_OBJECT));
		}
	
		[Test]
		public void GetFromInventoryConfigItemsCountTest()
		{
			AddMoqConfig();
			_inventory.AddItems(InventoryTypesEnum.TEST_OBJECT);
			Assert.NotZero(_inventory.ItemsCount(InventoryTypesEnum.TEST_OBJECT));
		}*/
	}
}