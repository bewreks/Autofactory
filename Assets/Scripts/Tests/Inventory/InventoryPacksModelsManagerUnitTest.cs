using System;
using Inventory;
using NUnit.Framework;
using Zenject;

namespace Tests.Inventory
{
    [TestFixture]
    public class InventoryPacksModelsManagerUnitTest : ZenjectUnitTestFixture
    {
        private InventoryPacksModelsManager _inventoryPacksModelsManager;

        public override void Setup()
        {
            base.Setup();
            _inventoryPacksModelsManager = new InventoryPacksModelsManager();
        }

        public override void Teardown()
        {
            base.Teardown();
            _inventoryPacksModelsManager = null;
        }

        [Test]
        public void TryGetNotConfigObject()
        {
            try
            {
                _inventoryPacksModelsManager.GetModel(InventoryTypesEnum.TEST_OBJECT);
            }
            catch (Exception)
            {
                Assert.Pass();
                throw;
            }
            Assert.Fail();
        }

        [Test]
        public void TryGetConfigObject()
        {
            _inventoryPacksModelsManager.Models.Add(InventoryPackModel.GetTestModel());
            try
            {
                _inventoryPacksModelsManager.GetModel(InventoryTypesEnum.TEST_OBJECT);
            }
            catch (Exception)
            {
                Assert.Fail();
                throw;
            }
            Assert.Pass();
        }
    }
}