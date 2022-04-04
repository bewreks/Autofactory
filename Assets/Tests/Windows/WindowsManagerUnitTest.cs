using System.Collections;
using Windows;
using Windows.TestWindows;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using Zenject;

namespace Tests.Windows
{
    [TestFixture]
    public class WindowsManagerUnitTest : ZenjectUnitTestFixture
    {
        public override void Setup()
        {
            base.Setup();
            
            var windowsSettings     = new WindowsSettings();
            var findAndSelectPrefab = Resources.Load<TestWindow>("Windows/TestWindow");
            windowsSettings.Windows.Add(findAndSelectPrefab);
            windowsSettings.Prepare();
            Container.Bind<WindowsSettings>().FromInstance(windowsSettings).AsSingle();
            Container.Bind<WindowsManager>().AsSingle();
        }

        public override void Teardown()
        {
            base.Teardown();
            Container.Unbind<WindowsSettings>();
        }
        
        [UnityTest]
        public IEnumerator OpenTestWindowTest()
        {
            var windowsManager = Container.Resolve<WindowsManager>();
            var testWindow     = windowsManager.OpenWindow<TestWindow>();
            Assert.AreEqual(WindowStateEnum.OPENING, testWindow.State.Value);
            yield return new WaitForSeconds(testWindow.Duration);
            Assert.AreEqual(WindowStateEnum.OPENED, testWindow.State.Value);
            testWindow.Close();
            Assert.AreEqual(WindowStateEnum.CLOSING, testWindow.State.Value);
            yield return new WaitForSeconds(testWindow.Duration);
            Assert.AreEqual(WindowStateEnum.CLOSED, testWindow.State.Value);
        }
        
        [Test]
        public void OpenNonConfigWindowTest()
        {
            var windowsManager = Container.Resolve<WindowsManager>();
            Assert.Null(windowsManager.OpenWindow<BrokenTestWindow>());
        }
    }
}