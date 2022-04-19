using System.Linq;
using Factories;
using Inventory;
using NUnit.Framework;
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
			Container.Unbind<InventoryPacksModelsSettings>();
			_inventory.Dispose();
			_inventory                    = null;
			_inventoryPacksModelsSettings = null;
		}

		private void AddMoqConfig()
		{
			_inventoryPacksModelsSettings.Models.Add(InventoryPackModel.GetTestModel());
		}

		[Test]
		public void AddToInventoryConfiguredItemTest([ValueSource("_addItemTestDatas")] InventoryTestData data)
		{
			AddMoqConfig();
			var resultType = data.Type;
			var addResult  = _inventory.AddItems(data.Type, data.ToChange);

			Assert.AreEqual(data.ResultChange,     addResult);
			Assert.AreEqual(data.ResultCount,      _inventory.ItemsCount(resultType));
			Assert.AreEqual(data.ResultCount,      _inventory.GetPacks().Sum(pack => pack.Size.Value));
			Assert.AreEqual(data.ResultCount,      _inventory.GetPacks(resultType).Count.Value);
			Assert.AreEqual(data.ResultPacksCount, _inventory.GetPacks().Count(pack => pack.Model.Type == resultType));
			Assert.AreEqual(data.ResultPacksCount, _inventory.GetPacks(resultType).Packs.Count());
		}

		[Test]
		public void RemoveToInventoryConfiguredItemTest([ValueSource("_removeTestData")] InventoryTestData data)
		{
			AddMoqConfig();
			var resultType = data.Type;
			_inventory.AddItems(resultType, data.InitCount);
			var removeResult = _inventory.RemoveItem(resultType, data.ToChange);

			Assert.AreEqual(data.ResultChange,     removeResult);
			Assert.AreEqual(data.ResultCount,      _inventory.ItemsCount(resultType));
			Assert.AreEqual(data.ResultCount,      _inventory.GetPacks().Sum(pack => pack.Size.Value));
			Assert.AreEqual(data.ResultCount,      _inventory.GetPacks(resultType).Count.Value);
			Assert.AreEqual(data.ResultPacksCount, _inventory.GetPacks().Count(pack => pack.Model.Type == resultType));
			Assert.AreEqual(data.ResultPacksCount, _inventory.GetPacks(resultType).Packs.Count());
		}

		[Test]
		public void AddToInventoryNotConfiguredItemTest(
			[ValueSource("_addItemNotConfiguredTestDatas")] InventoryTestData data)
		{
			var resultType = data.Type;
			var addResult  = _inventory.AddItems(data.Type, data.ToChange);

			Assert.AreEqual(data.ResultChange,     addResult);
			Assert.AreEqual(data.ResultCount,      _inventory.ItemsCount(resultType));
			Assert.AreEqual(data.ResultCount,      _inventory.GetPacks().Sum(pack => pack.Size.Value));
			Assert.AreEqual(data.ResultPacksCount, _inventory.GetPacks().Count(pack => pack.Model.Type == resultType));
			Assert.IsNull(_inventory.GetPacks(resultType));
		}

		[Test]
		public void RemoveToInventoryNotConfiguredItemTest(
			[ValueSource("_removeNotConfiguredTestData")] InventoryTestData data)
		{
			var resultType = data.Type;
			_inventory.AddItems(resultType, data.InitCount);
			var removeResult = _inventory.RemoveItem(resultType, data.ToChange);

			Assert.AreEqual(data.ResultChange,     removeResult);
			Assert.AreEqual(data.ResultCount,      _inventory.ItemsCount(resultType));
			Assert.AreEqual(data.ResultCount,      _inventory.GetPacks().Sum(pack => pack.Size.Value));
			Assert.AreEqual(data.ResultPacksCount, _inventory.GetPacks().Count(pack => pack.Model.Type == resultType));
			Assert.IsNull(_inventory.GetPacks(resultType));
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