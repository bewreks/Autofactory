using System.Collections;
using System.Linq;
using Installers;
using NUnit.Framework;
using UnityEngine.TestTools;
using Zenject;

namespace Tests.Integration.Windows
{
	public class WindowsIntegrationTest : ZenjectIntegrationTestFixture
	{
		[UnityTest]
		public IEnumerator WindowSettingsCountTest()
		{
			PreInstall();
			PostInstall();

			var windowsSettings = Container.Resolve<WindowsSettings>();
			Assert.NotZero(windowsSettings.Windows.Count);

			yield break;
		}

		[UnityTest]
		public IEnumerator WindowSettingsUniqueWindowsTest()
		{
			PreInstall();
			PostInstall();

			var windowsSettings = Container.Resolve<WindowsSettings>();
			var uniqueWindows   = windowsSettings.Windows.Select(window => window.GetType()).Distinct();
			var uniqueCount     = uniqueWindows.Count();
			if (uniqueCount != windowsSettings.Windows.Count)
			{
				var distinctItems = windowsSettings.Windows
				                                   .GroupBy(window => window.GetType())
				                                   .Where(g => g.Count() > 1)
				                                   .SelectMany(r => r);

				Assert.Fail(string.Join("\r\n", distinctItems));
			}

			yield break;
		}

		[UnityTest]
		public IEnumerator WindowSettingsPrepareTest()
		{
			PreInstall();
			PostInstall();

			var windowsSettings = Container.Resolve<WindowsSettings>();
			Assert.True(windowsSettings.Prepare());

			yield break;
		}
	}
}