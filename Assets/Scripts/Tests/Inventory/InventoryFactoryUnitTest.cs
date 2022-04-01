using Inventory;
using NUnit.Framework;
using Zenject;

namespace Tests.Inventory
{
    [TestFixture]
    public class InventoryFactoryUnitTest : ZenjectUnitTestFixture
    {
        public override void Teardown()
        {
            base.Teardown();
            InventoryFactory.Clear();
        }

        [Test]
        public void GetNewFromFactory()
        {
            Assert.NotNull(InventoryFactory.GetFactoryItem<InventoryPack>());
        }
        
        [Test]
        public void ReturnToFromFactory()
        {
            InventoryFactory.ReturnItem(new InventoryPack());
            Assert.NotZero(InventoryFactory.Objects[typeof(InventoryPack)].Count);
        }
        
        [Test]
        public void GetReturnedFromFactory()
        {
            var inventoryPack = new InventoryPack();
            InventoryFactory.ReturnItem(inventoryPack);
            Assert.NotZero(InventoryFactory.Objects[typeof(InventoryPack)].Count);
            Assert.AreEqual(InventoryFactory.GetFactoryItem<InventoryPack>(), inventoryPack);
        }
    }
}