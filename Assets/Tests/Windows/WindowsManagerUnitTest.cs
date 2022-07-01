using System.Collections;
using Windows;
using Windows.TestWindows;
using Installers;
using Moq;
using NUnit.Framework;
using Players;
using Players.Interfaces;
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
            Container.BindInterfacesTo<PlayerInputController>().FromNew().AsSingle();
            Container.Bind<WindowsSettings>().FromInstance(windowsSettings).AsSingle();
            Container.Bind<WindowsManager>().AsSingle();
        }

        public override void Teardown()
        {
            base.Teardown();
            Container.Unbind<WindowsSettings>();
            Container.Unbind<IPlayerInputController>();
        }
        
        [UnityTest]
        public IEnumerator OpenTestWindowTest()
        {
            var windowsManager = Container.Resolve<WindowsManager>();
            var windowWrapper     = windowsManager.OpenWindow<TestWindow>(IWindowManager.WindowOpenOption.Normal);
            Assert.AreEqual(WindowStateEnum.OPENING, windowWrapper.State);
            yield return new WaitForSeconds(windowWrapper.OpenDuration);
            Assert.AreEqual(WindowStateEnum.OPENED, windowWrapper.State);
            windowWrapper.Close();
            Assert.AreEqual(WindowStateEnum.CLOSING, windowWrapper.State);
            yield return new WaitForSeconds(windowWrapper.CloseDuration);
            Assert.AreEqual(WindowStateEnum.NOT_INITED, windowWrapper.State);
            yield return null;
        }
        
        [Test]
        public void OpenNonConfigWindowTest()
        {
            var windowsManager = Container.Resolve<WindowsManager>();
            Assert.Null(windowsManager.OpenWindow<WindowTest>(IWindowManager.WindowOpenOption.Normal));
        }
        
        private class WindowTest : Window
        {
            protected override IWindowController CreateWindowController()
            {
                return null;
            }
        }
    }
}