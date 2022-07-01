using System.Collections;
using Windows;
using Windows.TestWindows;
using Installers;
using Moq;
using NUnit.Framework;
using Players.Interfaces;
using UnityEngine;
using UnityEngine.TestTools;
using Zenject;

namespace Tests.Windows
{
	[TestFixture]
	public class WindowUnitTest : ZenjectUnitTestFixture
	{
		public override void Setup()
		{
			base.Setup();
			Container.Bind<IPlayerInputController>().FromInstance(null);
		}

		public override void Teardown()
		{
			base.Teardown();
			Container.Unbind<IPlayerInputController>();
		}

		[UnityTest]
		public IEnumerator TestWindowTest()
		{
			var windowWrapper = Resources.Load<TestWindow>("Windows/TestWindow");
			windowWrapper.Initialize(new Window.WindowData(), Container);
			Assert.AreEqual(WindowStateEnum.NOT_INITED, windowWrapper.State);
			windowWrapper.Open();
			Assert.AreEqual(WindowStateEnum.OPENING, windowWrapper.State);
			yield return new WaitForSeconds(windowWrapper.OpenDuration);
			Assert.AreEqual(WindowStateEnum.OPENED, windowWrapper.State);
			windowWrapper.Close();
			Assert.AreEqual(WindowStateEnum.CLOSING, windowWrapper.State);
			yield return new WaitForSeconds(windowWrapper.CloseDuration);
			Assert.AreEqual(WindowStateEnum.NOT_INITED, windowWrapper.State);
			yield return null;
		}

		[UnityTest]
		public IEnumerator ReopenTestWindowTest()
		{
			var windowWrapper = Resources.Load<TestWindow>("Windows/TestWindow");
			windowWrapper.Initialize(new Window.WindowData(), Container);
			Assert.AreEqual(WindowStateEnum.NOT_INITED, windowWrapper.State);
			windowWrapper.Open();
			Assert.AreEqual(WindowStateEnum.OPENING, windowWrapper.State);
			yield return new WaitForSeconds(windowWrapper.OpenDuration);
			Assert.AreEqual(WindowStateEnum.OPENED, windowWrapper.State);
			windowWrapper.Close();
			Assert.AreEqual(WindowStateEnum.CLOSING, windowWrapper.State);
			yield return new WaitForSeconds(windowWrapper.CloseDuration);
			Assert.AreEqual(WindowStateEnum.NOT_INITED, windowWrapper.State);
			yield return null;
			var windowWrapper1 = Resources.Load<TestWindow>("Windows/TestWindow");
			windowWrapper1.Initialize(new Window.WindowData(), Container);
			Assert.AreEqual(WindowStateEnum.NOT_INITED, windowWrapper1.State);
			windowWrapper1.Open();
			Assert.AreEqual(WindowStateEnum.OPENING, windowWrapper1.State);
			yield return new WaitForSeconds(windowWrapper1.OpenDuration);
			Assert.AreEqual(WindowStateEnum.OPENED, windowWrapper1.State);
			windowWrapper1.Close();
			Assert.AreEqual(WindowStateEnum.CLOSING, windowWrapper1.State);
			yield return new WaitForSeconds(windowWrapper1.CloseDuration);
			Assert.AreEqual(WindowStateEnum.NOT_INITED, windowWrapper1.State);
			yield return null;
		}
	}
}