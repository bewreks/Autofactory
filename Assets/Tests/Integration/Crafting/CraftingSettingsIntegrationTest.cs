using System.Collections;
using System.Linq;
using Installers;
using NUnit.Framework;
using UnityEngine.TestTools;
using Zenject;

namespace Tests.Integration.Crafting
{
	public class CraftingSettingsIntegrationTest : ZenjectIntegrationTestFixture
	{
		[UnityTest]
		public IEnumerator SettingsNotEmptyTest()
		{
			PreInstall();
			PostInstall();

			var craftSettings = Container.Resolve<CraftSettings>();
			Assert.NotZero(craftSettings.Models.Count);

			yield break;
		}

		[UnityTest]
		public IEnumerator SettingsPrepareTest()
		{
			PreInstall();
			PostInstall();

			var craftSettings = Container.Resolve<CraftSettings>();
			Assert.IsTrue(craftSettings.Prepare());

			yield break;
		}

		[UnityTest]
		public IEnumerator SettingsUniqueTypeTest()
		{
			PreInstall();
			PostInstall();

			var craftSettings = Container.Resolve<CraftSettings>();
			var uniqueWindows = craftSettings.Models.Select(model => model.CraftingResult.model.Type).Distinct();
			var uniqueCount   = uniqueWindows.Count();
			if (uniqueCount != craftSettings.Models.Count)
			{
				var distinctItems = craftSettings.Models
				                                 .GroupBy(model => model.CraftingResult.model.Type)
				                                 .Where(g => g.Count() > 1)
				                                 .SelectMany(r => r);

				Assert.Fail(string.Join("\r\n", distinctItems));
			}

			yield break;
		}
	}
}