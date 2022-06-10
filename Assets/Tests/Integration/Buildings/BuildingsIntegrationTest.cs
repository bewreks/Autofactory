using System.Collections;
using System.Linq;
using Installers;
using NUnit.Framework;
using UnityEngine.TestTools;
using Zenject;

namespace Tests.Integration.Buildings
{
    public class BuildingsIntegrationTest : ZenjectIntegrationTestFixture
    {
        [UnityTest]
        public IEnumerator SettingsNotEmptyTest()
        {
            PreInstall();
            PostInstall();

            var buildingsModelsSettings = Container.Resolve<BuildingsModelsSettings>();
            Assert.NotZero(buildingsModelsSettings.Models.Count);

            yield break;
        }

        [UnityTest]
        public IEnumerator SettingsPrepareTest()
        {
            PreInstall();
            PostInstall();

            var craftSettings = Container.Resolve<BuildingsModelsSettings>();
            Assert.IsTrue(craftSettings.Prepare());

            yield break;
        }

        [UnityTest]
        public IEnumerator SettingsUniqueTypeTest()
        {
            PreInstall();
            PostInstall();

            var craftSettings = Container.Resolve<BuildingsModelsSettings>();
            var uniqueWindows = craftSettings.Models.Select(model => model.Type).Distinct();
            var uniqueCount   = uniqueWindows.Count();
            if (uniqueCount != craftSettings.Models.Count)
            {
                var distinctItems = craftSettings.Models
                                                 .GroupBy(model => model.Type)
                                                 .Where(g => g.Count() > 1)
                                                 .SelectMany(r => r);

                Assert.Fail(string.Join("\r\n", distinctItems));
            }

            yield break;
        }
    }
}