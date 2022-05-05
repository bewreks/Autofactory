using System.Linq;
using Windows;
using Windows.TestWindows;
using Editor;
using Installers;
using NUnit.Framework;
using UnityEngine;
using Zenject;

namespace Tests.Windows
{
    [TestFixture]
    public class WindowsSettingsUnitTest : ZenjectUnitTestFixture
    {
        private WindowsSettings _windowsSettings;

        public override void Setup()
        {
            base.Setup();
            _windowsSettings = new WindowsSettings();
        }

        public override void Teardown()
        {
            base.Teardown();
            _windowsSettings = null;
        }

        [Test]
        public void TryGetFromNotPreparedSettingsTest()
        {
            Assert.Null(_windowsSettings.GetWindow<TestWindow>());
        }

        [Test]
        public void TryGetNotConfigWindowTest()
        {
            var findAndSelectPrefab = Resources.Load<BrokenTestWindow>("Windows/BrokenTestWindow");
            _windowsSettings.Windows.Add(findAndSelectPrefab);
            _windowsSettings.Prepare();
            Assert.Null(_windowsSettings.GetWindow<TestWindow>());
        }

        [Test]
        public void TryGetConfigWindowTest()
        {
            var findAndSelectPrefab = Resources.Load<TestWindow>("Windows/TestWindow");
            _windowsSettings.Windows.Add(findAndSelectPrefab);
            _windowsSettings.Prepare();
            Assert.NotNull(_windowsSettings.GetWindow<TestWindow>());
        }
    }
}