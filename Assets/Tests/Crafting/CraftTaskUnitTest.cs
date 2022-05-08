using Crafting;
using Factories;
using Installers;
using Inventories;
using NUnit.Framework;
using UnityEngine;
using Zenject;

namespace Tests.Crafting
{
	[TestFixture]
	public class CraftTaskUnitTest : ZenjectUnitTestFixture
	{
		#region Test Data classes

		public class CraftTestData
		{
			public float TimeOfTick;
			public int   StartNothing;
			public int   StartTestObject;
			public int   ResultNothing;
			public int   ResultTestObject;
		}

		public static CraftTestData[] _craftTests =
		{
			new CraftTestData
			{
				TimeOfTick       = 0.1f,
				ResultNothing    = 0,
				StartNothing     = 1,
				ResultTestObject = 1,
				StartTestObject  = 0
			},
			new CraftTestData
			{
				TimeOfTick       = 1f,
				ResultNothing    = 0,
				StartNothing     = 1,
				ResultTestObject = 1,
				StartTestObject  = 0
			},
			new CraftTestData
			{
				TimeOfTick       = 1.1f,
				ResultNothing    = 0,
				StartNothing     = 1,
				ResultTestObject = 1,
				StartTestObject  = 0
			},
		};

		#endregion

		private IInventory                   _inventory;
		private CraftingTask                 _craftingTask;
		private InventoryPacksModelsSettings _inventoryPacksModelsSettings;

		public override void Setup()
		{
			base.Setup();

			_inventory                    = new Inventories.Inventory();
			_inventoryPacksModelsSettings = new InventoryPacksModelsSettings();
			_craftingTask                 = Factory.GetFactoryItem<CraftingTask>();
			_inventoryPacksModelsSettings.Inventories.Add(InventoryTypesModel.GetUnlimitedTestModel());
			_inventoryPacksModelsSettings.Models.Add(InventoryPackModel.GetNothingTestModel());
			_inventoryPacksModelsSettings.Models.Add(InventoryPackModel.GetTestModel());
			_inventoryPacksModelsSettings.Prepare();

			Container.Bind<InventoryPacksModelsSettings>().FromInstance(_inventoryPacksModelsSettings).AsSingle();
			Container.Inject(_inventory);
		}

		public override void Teardown()
		{
			base.Teardown();

			Factory.Clear();

			Container.Unbind<InventoryPacksModelsSettings>();

			_inventory.Dispose();
			_craftingTask.Dispose();
			_inventory                    = null;
			_craftingTask                 = null;
			_inventoryPacksModelsSettings = null;
		}

		[Test]
		public void CraftTaskTest([ValueSource("_craftTests")] CraftTestData data)
		{
			var taskCompleted = false;
			_inventory.AddItems(InventoryObjectsTypesEnum.NOTHING, data.StartNothing, out var edge);
			Assert.AreEqual(_inventory.ItemsCount(InventoryObjectsTypesEnum.NOTHING),     data.StartNothing);
			Assert.AreEqual(_inventory.ItemsCount(InventoryObjectsTypesEnum.TEST_OBJECT), data.StartTestObject);
			var craftingModel = CraftingModel.GetTestModel();
			_craftingTask.Initialize(_inventory, _inventory, craftingModel);
			_craftingTask.TaskComplete += task => { taskCompleted = true; };
			for (var i = 0; i < Mathf.Ceil(craftingModel.CraftingTime / data.TimeOfTick); i++)
			{
				_craftingTask.Tick(data.TimeOfTick);
			}
			Assert.AreEqual(_inventory.ItemsCount(InventoryObjectsTypesEnum.NOTHING),     data.ResultNothing);
			Assert.AreEqual(_inventory.ItemsCount(InventoryObjectsTypesEnum.TEST_OBJECT), data.ResultTestObject);
			Assert.IsTrue(taskCompleted);
		}
	}
}