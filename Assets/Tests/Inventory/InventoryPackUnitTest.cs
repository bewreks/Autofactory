using System;
using Inventory;
using NUnit.Framework;
using UniRx;
using Zenject;

namespace Tests.Inventory
{
    [TestFixture]
    public class InventoryPackUnitTest : ZenjectUnitTestFixture
    {
        [Test]
        public void PackFullEventTest()
        {
            var increment     = true;
            var inventoryPack = new InventoryPack();
            inventoryPack.Initialize(InventoryPackModel.GetTestModel(), 0);
            inventoryPack.PackIsFull += () =>
            {
                increment = false;
                Assert.Pass();
            };
            while (increment)
            {
                inventoryPack.AddItem();
            }
        }
        
        [Test]
        public void PackEmptyEventTest()
        {
            var increment     = true;
            var inventoryPack = new InventoryPack();
            inventoryPack.Initialize(InventoryPackModel.GetTestModel(), 10);
            inventoryPack.PackIsEmpty += () =>
            {
                increment = false;
                Assert.Pass();
            };
            while (increment)
            {
                inventoryPack.RemoveItem();
            }
        }
        
        [Test]
        public void PackSizeChangedEventTest()
        {
            var testSize      = 10;
            var inventoryPack = new InventoryPack();
            inventoryPack.Initialize(InventoryPackModel.GetTestModel(), testSize);
            IDisposable disposable = null;
            disposable = inventoryPack.Size.Subscribe(i =>
            {
                Assert.AreEqual(testSize, i);
                // ReSharper disable once AccessToModifiedClosure
                disposable?.Dispose();
            });
            testSize = 9;
            inventoryPack.RemoveItem();
        }
        
        [Test]
        public void PackAddItemTest()
        {
            var inventoryPack = new InventoryPack();
            inventoryPack.Initialize(InventoryPackModel.GetTestModel(), 0);
            Assert.True(inventoryPack.AddItem());
            Assert.AreEqual(1, inventoryPack.Size.Value);
        }
        
        [Test]
        public void PackRemoveTest()
        {
            var inventoryPack = new InventoryPack();
            inventoryPack.Initialize(InventoryPackModel.GetTestModel(), 2);
            Assert.True(inventoryPack.RemoveItem());
            Assert.AreEqual(1, inventoryPack.Size.Value);
        }
        
        [Test]
        public void PackAddItemToFullTest()
        {
            var inventoryPack      = new InventoryPack();
            var inventoryPackModel = InventoryPackModel.GetTestModel();
            var maxPackSize        = inventoryPackModel.MaxPackSize;
            inventoryPack.Initialize(inventoryPackModel, maxPackSize);
            Assert.False(inventoryPack.AddItem());
            Assert.AreEqual(maxPackSize, inventoryPack.Size.Value);
        }
        
        [Test]
        public void PackRemoveFromEmptyTest()
        {
            var inventoryPack      = new InventoryPack();
            var inventoryPackModel = InventoryPackModel.GetTestModel();
            inventoryPack.Initialize(inventoryPackModel, 0);
            Assert.False(inventoryPack.RemoveItem());
            Assert.AreEqual(0, inventoryPack.Size.Value);
        }
    }
}