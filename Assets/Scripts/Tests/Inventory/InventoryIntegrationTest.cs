using System.Collections;
using System.Linq;
using Inventory;
using NUnit.Framework;
using UnityEngine.TestTools;
using Zenject;

namespace Tests.Inventory
{
	public class InventoryIntegrationTest : ZenjectIntegrationTestFixture
	{
		[UnityTest]
		public IEnumerator InventoryManagerCountTest()
		{
			PreInstall();
			PostInstall();

			var inventoryPacksModelsManager = Container.Resolve<InventoryPacksModelsManager>();
			Assert.NotZero(inventoryPacksModelsManager.Models.Count);

			yield break;
		}

		[UnityTest]
		public IEnumerator InventoryManagerContentTest()
		{
			PreInstall();
			PostInstall();

			var inventoryPacksModelsManager = Container.Resolve<InventoryPacksModelsManager>();
			foreach (var packModel in inventoryPacksModelsManager.Models)
			{
				Assert.NotNull(packModel.Icon, $"packModel.Icon != null at {packModel.name}");
				Assert.NotZero(packModel.MaxPackSize, $"packModel.MaxPackSize != 0 at {packModel.name}");
				Assert.AreNotEqual(InventoryTypesEnum.NOTHING, packModel.Type,
				                   $"packModel.Type != NOTHING at {packModel.name}");
			}

			yield break;
		}

		[UnityTest]
		public IEnumerator InventoryManagerContentUniqueTypesTest()
		{
			PreInstall();
			PostInstall();

			var inventoryPacksModelsManager = Container.Resolve<InventoryPacksModelsManager>();
			var inventoryTypesEnums         = inventoryPacksModelsManager.Models.Select(model => model.Type);
			var inventoryUniqueTypes        = inventoryTypesEnums.Distinct();
			var uniqueCount                 = inventoryUniqueTypes.Count();
			if (uniqueCount != inventoryPacksModelsManager.Models.Count)
			{
				var distinctItems = inventoryPacksModelsManager
				                    .Models
				                    .GroupBy(model => model.Type)
				                    .Where(g => g.Count() > 1)
				                    .SelectMany(r => r);

				Assert.Fail(string.Join("\r\n", distinctItems));
			}

			yield break;
		}

		[UnityTest]
		public IEnumerator InventoryManagerContentDuplicatesTest()
		{
			PreInstall();
			PostInstall();

			var inventoryPacksModelsManager = Container.Resolve<InventoryPacksModelsManager>();
			var inventoryUniqueTypes        = inventoryPacksModelsManager.Models.Select(model => model.Type).Distinct();
			if (inventoryUniqueTypes.Count() != inventoryPacksModelsManager.Models.Count)
			{
				var distinctItems = inventoryPacksModelsManager
				                    .Models
				                    .GroupBy(model => model)
				                    .Where(g => g.Count() > 1)
				                    .SelectMany(r => r);

				Assert.Fail(string.Join("\r\n", distinctItems));
			}

			yield break;
		}
	}
}