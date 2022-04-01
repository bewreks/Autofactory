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
    public class WindowUnitTest : ZenjectUnitTestFixture
    {
        [UnityTest]
        public IEnumerator TestWindowTest()
        {
            var findAndSelectPrefab = Resources.Load<TestWindow>("Windows/TestWindow");
            var testWindow          = Object.Instantiate(findAndSelectPrefab).GetComponent<TestWindow>();
            Assert.AreEqual(WindowStateEnum.CLOSED, testWindow.State.Value);
            testWindow.Open();
            Assert.AreEqual(WindowStateEnum.OPENING, testWindow.State.Value);
            yield return new WaitForSeconds(testWindow.Duration);
            Assert.AreEqual(WindowStateEnum.OPENED, testWindow.State.Value);
            testWindow.Close();
            Assert.AreEqual(WindowStateEnum.CLOSING, testWindow.State.Value);
            yield return new WaitForSeconds(testWindow.Duration);
            Assert.AreEqual(WindowStateEnum.CLOSED, testWindow.State.Value);
        }
        
        [UnityTest]
        public IEnumerator BrokenTestWindowTest()
        {
            var findAndSelectPrefab = Resources.Load<BrokenTestWindow>("Windows/BrokenTestWindow");
            var testWindow          = Object.Instantiate(findAndSelectPrefab).GetComponent<BrokenTestWindow>();
            Assert.AreEqual(WindowStateEnum.CLOSED, testWindow.State.Value);
            testWindow.Open();
            Assert.AreEqual(WindowStateEnum.OPENING, testWindow.State.Value);
            yield return new WaitForSeconds(testWindow.Duration);
            Assert.AreEqual(WindowStateEnum.OPENING, testWindow.State.Value);
            testWindow.Close();
            Assert.AreEqual(WindowStateEnum.CLOSING, testWindow.State.Value);
            yield return new WaitForSeconds(testWindow.Duration);
            Assert.AreEqual(WindowStateEnum.CLOSING, testWindow.State.Value);
        }
    }
}