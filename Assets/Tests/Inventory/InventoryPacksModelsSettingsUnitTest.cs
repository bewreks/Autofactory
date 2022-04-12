using System;
using Inventory;
using NUnit.Framework;
using Zenject;

namespace Tests.Inventory
{
    [TestFixture]
    public class InventoryPacksModelsSettingsUnitTest : ZenjectUnitTestFixture
    {
        private InventoryPacksModelsSettings _inventoryPacksModelsSettings;

        public override void Setup()
        {
            base.Setup();
            _inventoryPacksModelsSettings = new InventoryPacksModelsSettings();
        }

        public override void Teardown()
        {
            base.Teardown();
            _inventoryPacksModelsSettings = null;
        }

        [Test]
        public void TryGetNotConfigObject()
        {
            try
            {
                _inventoryPacksModelsSettings.GetModel(InventoryTypesEnum.TEST_OBJECT);
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
            _inventoryPacksModelsSettings.Models.Add(InventoryPackModel.GetTestModel());
            try
            {
                _inventoryPacksModelsSettings.GetModel(InventoryTypesEnum.TEST_OBJECT);
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