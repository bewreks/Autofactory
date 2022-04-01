using Inventory;
using NUnit.Framework;
using Zenject;

namespace Tests.Inventory
{
	[TestFixture]
	public class InventoryUnitTest : ZenjectUnitTestFixture
	{
		private IInventory                  _inventory;
		private InventoryPacksModelsManager _inventoryPacksModelsManager;

		public override void Setup()
		{
			base.Setup();
			_inventoryPacksModelsManager = new InventoryPacksModelsManager();
			Container.Bind<InventoryPacksModelsManager>().FromInstance(_inventoryPacksModelsManager).AsSingle();

			_inventory = new global::Inventory.Inventory();
			Container.Inject(_inventory);
		}

		public override void Teardown()
		{
			base.Teardown();
			Container.Unbind<InventoryPacksModelsManager>();
			_inventory.Dispose();
			_inventory                   = null;
			_inventoryPacksModelsManager = null;
		}

		[Test]
		public void AddToInventoryNotConfigItemTest()
		{
			Assert.False(_inventory.AddItem(InventoryTypesEnum.TEST_OBJECT));
		}

		[Test]
		public void RemoveFromInventoryNotConfigItemTest()
		{
			Assert.False(_inventory.RemoveItem(InventoryTypesEnum.TEST_OBJECT));
		}

		[Test]
		public void GetFromInventoryNotConfigItemsCountTest()
		{
			Assert.Zero(_inventory.GetItemsCount(InventoryTypesEnum.TEST_OBJECT));
		}

		private void AddMoqConfig()
		{
			_inventoryPacksModelsManager.Models.Add(InventoryPackModel.GetTestModel());
		}

		[Test]
		public void AddToInventoryConfigItemTest()
		{
			AddMoqConfig();
			Assert.True(_inventory.AddItem(InventoryTypesEnum.TEST_OBJECT));
		}

		[Test]
		public void RemoveFromInventoryConfigItemTest()
		{
			AddMoqConfig();
			_inventory.AddItem(InventoryTypesEnum.TEST_OBJECT);
			Assert.True(_inventory.RemoveItem(InventoryTypesEnum.TEST_OBJECT));
		}

		[Test]
		public void GetFromInventoryConfigItemsCountTest()
		{
			AddMoqConfig();
			_inventory.AddItem(InventoryTypesEnum.TEST_OBJECT);
			Assert.NotZero(_inventory.GetItemsCount(InventoryTypesEnum.TEST_OBJECT));
		}
	}
}