using Factories;
using NUnit.Framework;
using Zenject;

namespace Tests
{
    [TestFixture]
    public class FactoryUnitTest : ZenjectUnitTestFixture
    {
        public override void Teardown()
        {
            base.Teardown();
            Factory.Clear();
        }

        [Test]
        public void GetNewFromFactory()
        {
            Assert.NotNull(Factory.GetFactoryItem<TestFactoryClass>());
        }

        [Test]
        public void GetInjectedNewFromFactory()
        {
            Container.Bind<TestFactoryClass>().AsSingle();
            var factoryItem = Factory.GetFactoryItem<TestInjectFactoryClass>(Container);
            Assert.NotNull(factoryItem);
            Assert.NotNull(factoryItem.test);
        }
        
        [Test]
        public void ReturnToFromFactory()
        {
            Factory.ReturnItem(new TestFactoryClass());
            Assert.NotZero(Factory.Objects[typeof(TestFactoryClass)].Count);
        }
        
        [Test]
        public void GetReturnedFromFactory()
        {
            var inventoryPack = new TestFactoryClass();
            Factory.ReturnItem(inventoryPack);
            Assert.NotZero(Factory.Objects[typeof(TestFactoryClass)].Count);
            Assert.AreEqual(Factory.GetFactoryItem<TestFactoryClass>(), inventoryPack);
        }
    }

    internal class TestFactoryClass
    {
    }

    internal class TestInjectFactoryClass
    {
        [Inject] public TestFactoryClass test;
    }
}