using Inventory;
using NUnit.Framework;
using Zenject;

namespace Tests.Inventory
{
	[TestFixture]
	public class InventoryUnitTest : ZenjectUnitTestFixture
	{
		private IInventory                  _inventory;
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
			_inventory                   = null;
			_inventoryPacksModelsSettings = null;
		}

		[Test]
		public void AddToInventoryNotConfigItemTest()
		{
			Assert.False(_inventory.AddItems(InventoryTypesEnum.TEST_OBJECT));
		}

		[Test]
		public void RemoveFromInventoryNotConfigItemTest()
		{
			Assert.False(_inventory.RemoveItem(InventoryTypesEnum.TEST_OBJECT));
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