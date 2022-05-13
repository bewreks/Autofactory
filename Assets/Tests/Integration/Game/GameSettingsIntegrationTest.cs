using System.Collections;
using Installers;
using NUnit.Framework;
using UnityEngine.TestTools;
using Zenject;

namespace Tests.Integration.Game
{
    public class GameSettingsIntegrationTest : ZenjectIntegrationTestFixture
    {
        [UnityTest]
        public IEnumerator PlayerGameSettingsTest()
        {
            PreInstall();
            PostInstall();

            var gameSettings = Container.Resolve<GameSettings>();
            Assert.NotZero(gameSettings.MoveSpeed);
            Assert.NotZero(gameSettings.RotationSpeed);
            Assert.NotNull(gameSettings.PlayerPrefab);

            yield break;
        }
        
        [UnityTest]
        public IEnumerator GlobalGameSettingsTest()
        {
            PreInstall();
            PostInstall();

            var gameSettings = Container.Resolve<GameSettings>();
            Assert.NotZero(gameSettings.GroundMask.value);

            yield break;
        }
    }
}