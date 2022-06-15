using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Crafting;
using Installers;
using ModestTree;
using UnityEngine.TestTools;
using Zenject;
using Assert = NUnit.Framework.Assert;

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
			var uniqueModels  = craftSettings.Models.Select(model => model.CraftingResult.model.Type).Distinct();
			var uniqueCount   = uniqueModels.Count();
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

		[UnityTest]
		public IEnumerator SettingsConfiguredNeedsTest()
		{
			PreInstall();
			PostInstall();

			var craftSettings = Container.Resolve<CraftSettings>();
			var errorsInNeeds = craftSettings.Models
			                                 .Where(model => model.CraftingNeeds.Any(need => need == null ||
			                                                                                 need.model == null))
			                                 .Select(model => model.name)
			                                 .ToArray();
			if (!errorsInNeeds.IsEmpty())
			{
				Assert.Fail(string.Join("\r\n", errorsInNeeds));
			}

			yield break;
		}

		[UnityTest]
		public IEnumerator SettingsConfiguredResultsTest()
		{
			PreInstall();
			PostInstall();

			var craftSettings = Container.Resolve<CraftSettings>();
			var errorsInNeeds = craftSettings.Models
			                                 .Where(model => model.CraftingResult == null ||
			                                                 model.CraftingResult.model == null)
			                                 .Select(model => model.name)
			                                 .ToArray();
			if (!errorsInNeeds.IsEmpty())
			{
				Assert.Fail(string.Join("\r\n", errorsInNeeds));
			}

			yield break;
		}
	}
}