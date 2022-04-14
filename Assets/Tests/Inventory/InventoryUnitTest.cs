using Factories;
using Inventory;
using NUnit.Framework;
using Zenject;

namespace Tests.Inventory
{
	[TestFixture]
	public class InventoryUnitTest : ZenjectUnitTestFixture
	{
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

		[Test]
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

		private void AddMoqConfig()
		{
			_inventoryPacksModelsSettings.Models.Add(InventoryPackModel.GetTestModel());
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
		}
	}
}