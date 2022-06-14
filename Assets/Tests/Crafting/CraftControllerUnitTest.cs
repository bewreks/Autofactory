using System.Collections;
using Crafting;
using Factories;
using Installers;
using Inventories;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using Zenject;

namespace Tests.Crafting
{
    [TestFixture]
    public class CraftControllerUnitTest : ZenjectUnitTestFixture
    {
        private IInventory                   _inventory;
        private CraftingController           _craftingController;
        private InventoryPacksModelsSettings _inventoryPacksModelsSettings;
        private CraftSettings                _craftSettings;

        public override void Setup()
        {
            base.Setup();

            _inventory                    = new Inventories.Inventory();
            _inventoryPacksModelsSettings = new InventoryPacksModelsSettings();
            _craftingController           = new CraftingController();
            _craftSettings                = new CraftSettings();
            _inventoryPacksModelsSettings.Inventories.Add(InventoryTypesModel.GetUnlimitedTestModel());
            _inventoryPacksModelsSettings.Models.Add(InventoryPackModel.GetNothingTestModel());
            _inventoryPacksModelsSettings.Models.Add(InventoryPackModel.GetTestModel());
            _inventoryPacksModelsSettings.Prepare();
            _craftSettings.Models.Add(CraftingModel.GetTestModel());
            _craftSettings.Prepare();

            Container.Bind<InventoryPacksModelsSettings>().FromInstance(_inventoryPacksModelsSettings).AsSingle();
            Container.Bind<CraftSettings>().FromInstance(_craftSettings).AsSingle();
            Container.Inject(_craftingController);
            Container.Inject(_inventory);
        }

        public override void Teardown()
        {
            base.Teardown();

            Factory.Clear();

            Container.Unbind<InventoryPacksModelsSettings>();

            _inventory.Dispose();
            _craftingController.Dispose();
            _inventory                    = null;
            _craftingController           = null;
            _inventoryPacksModelsSettings = null;
        }
    
        [UnityTest]
        public IEnumerator StartOfCraftTest()
        {
            _inventory.AddItems(InventoryObjectsTypesEnum.NOTHING, 1, out var edge);
            Assert.AreEqual(_inventory.ItemsCount(InventoryObjectsTypesEnum.NOTHING),     1);
            Assert.AreEqual(_inventory.ItemsCount(InventoryObjectsTypesEnum.TEST_OBJECT), 0);
            _craftingController.StartCraft(_inventory, _inventory, InventoryObjectsTypesEnum.TEST_OBJECT);
            yield return new WaitForSeconds(1.1f);
            Assert.AreEqual(_inventory.ItemsCount(InventoryObjectsTypesEnum.NOTHING),     0);
            Assert.AreEqual(_inventory.ItemsCount(InventoryObjectsTypesEnum.TEST_OBJECT), 1);
        }
    
        [UnityTest]
        public IEnumerator StartOfZeroTimeCraftTest()
        {
            _craftSettings.Models.Clear();
            _craftSettings.Models.Add(CraftingModel.GetZeroTestModel());
            _craftSettings.Prepare();
            _inventory.AddItems(InventoryObjectsTypesEnum.NOTHING, 1, out var edge);
            Assert.AreEqual(_inventory.ItemsCount(InventoryObjectsTypesEnum.NOTHING),     1);
            Assert.AreEqual(_inventory.ItemsCount(InventoryObjectsTypesEnum.TEST_OBJECT), 0);
            _craftingController.StartCraft(_inventory, _inventory, InventoryObjectsTypesEnum.TEST_OBJECT);
            yield return new WaitForSeconds(.1f);
            Assert.AreEqual(_inventory.ItemsCount(InventoryObjectsTypesEnum.NOTHING),     0);
            Assert.AreEqual(_inventory.ItemsCount(InventoryObjectsTypesEnum.TEST_OBJECT), 1);
        }
    
        [UnityTest]
        public IEnumerator StartOfCraftWithoutStartTest()
        {
            Assert.AreEqual(_inventory.ItemsCount(InventoryObjectsTypesEnum.NOTHING),     0);
            Assert.AreEqual(_inventory.ItemsCount(InventoryObjectsTypesEnum.TEST_OBJECT), 0);
            _craftingController.StartCraft(_inventory, _inventory, InventoryObjectsTypesEnum.TEST_OBJECT);
            yield return new WaitForSeconds(1);
            Assert.AreEqual(_inventory.ItemsCount(InventoryObjectsTypesEnum.NOTHING),     0);
            Assert.AreEqual(_inventory.ItemsCount(InventoryObjectsTypesEnum.TEST_OBJECT), 0);
        }
    
        [UnityTest]
        public IEnumerator CancelOneOfCraftTest()
        {
            _inventory.AddItems(InventoryObjectsTypesEnum.NOTHING, 1, out var edge);
            Assert.AreEqual(_inventory.ItemsCount(InventoryObjectsTypesEnum.NOTHING),     1);
            Assert.AreEqual(_inventory.ItemsCount(InventoryObjectsTypesEnum.TEST_OBJECT), 0);
            _craftingController.StartCraft(_inventory, _inventory, InventoryObjectsTypesEnum.TEST_OBJECT);
            yield return new WaitForSeconds(.1f);
            _craftingController.CancelCraft(_inventory, _inventory);
            yield return new WaitForSeconds(.1f);
            Assert.AreEqual(_inventory.ItemsCount(InventoryObjectsTypesEnum.NOTHING),     1);
            Assert.AreEqual(_inventory.ItemsCount(InventoryObjectsTypesEnum.TEST_OBJECT), 0);
        }
    
        [UnityTest]
        public IEnumerator CancelTwoOfCraftTest()
        {
            _inventory.AddItems(InventoryObjectsTypesEnum.NOTHING, 2, out var edge);
            Assert.AreEqual(_inventory.ItemsCount(InventoryObjectsTypesEnum.NOTHING),     2);
            Assert.AreEqual(_inventory.ItemsCount(InventoryObjectsTypesEnum.TEST_OBJECT), 0);
            _craftingController.StartCraft(_inventory, _inventory, InventoryObjectsTypesEnum.TEST_OBJECT);
            yield return new WaitForSeconds(.1f);
            _craftingController.StartCraft(_inventory, _inventory, InventoryObjectsTypesEnum.TEST_OBJECT);
            yield return new WaitForSeconds(.4f);
            _craftingController.CancelCraft(_inventory, _inventory);
            yield return new WaitForSeconds(.1f);
            Assert.AreEqual(_inventory.ItemsCount(InventoryObjectsTypesEnum.NOTHING),     2);
            Assert.AreEqual(_inventory.ItemsCount(InventoryObjectsTypesEnum.TEST_OBJECT), 0);
        }
    }
}