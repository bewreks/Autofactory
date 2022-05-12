using System.Collections;
using System.Linq;
using Installers;
using Inventories;
using NUnit.Framework;
using UnityEngine.TestTools;
using Zenject;

namespace Tests.Intergration.Inventory
{
	public class InventoryIntegrationTest : ZenjectIntegrationTestFixture
	{
		[UnityTest]
		public IEnumerator InventoryManagerCountTest()
		{
			PreInstall();
			PostInstall();

			var inventoryPacksModelsManager = Container.Resolve<InventoryPacksModelsSettings>();
			Assert.NotZero(inventoryPacksModelsManager.Models.Count);

			yield break;
		}
		
		[UnityTest]
		public IEnumerator InventoryTypesCountTest()
		{
			PreInstall();
			PostInstall();

			var inventoryPacksModelsManager = Container.Resolve<InventoryPacksModelsSettings>();
			Assert.NotZero(inventoryPacksModelsManager.Inventories.Count);

			yield break;
		}

		[UnityTest]
		public IEnumerator InventoryManagerContentTest()
		{
			PreInstall();
			PostInstall();

			var inventoryPacksModelsManager = Container.Resolve<InventoryPacksModelsSettings>();
			foreach (var packModel in inventoryPacksModelsManager.Models)
			{
				Assert.NotNull(packModel.Icon, $"packModel.Icon != null at {packModel.name}");
				Assert.NotZero(packModel.MaxPackSize, $"packModel.MaxPackSize != 0 at {packModel.name}");
				Assert.AreNotEqual(InventoryObjectsTypesEnum.NOT_CONFIGURED, packModel.Type,
				                   $"packModel.Type != NOTHING at {packModel.name}");
			}

			yield break;
		}

		[UnityTest]
		public IEnumerator InventoryTypesContentTest()
		{
			PreInstall();
			PostInstall();

			var inventoryPacksModelsManager = Container.Resolve<InventoryPacksModelsSettings>();
			foreach (var inventoryModel in inventoryPacksModelsManager.Inventories)
			{
				Assert.NotZero(inventoryModel.Limit, $"inventoryModel.Limit != 0 at {inventoryModel.name}");
				Assert.AreNotEqual(InventoryTypesEnum.TEST, inventoryModel.InventoryType,
				                   $"inventoryModel.Type != TEST at {inventoryModel.name}");
			}

			yield break;
		}

		[UnityTest]
		public IEnumerator InventoryManagerContentUniqueTypesTest()
		{
			PreInstall();
			PostInstall();

			var inventoryPacksModelsManager = Container.Resolve<InventoryPacksModelsSettings>();
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
		public IEnumerator InventoryTypesContentUniqueTypesTest()
		{
			PreInstall();
			PostInstall();

			var inventoryTypesModelsManager = Container.Resolve<InventoryPacksModelsSettings>();
			var inventoryTypesEnums         = inventoryTypesModelsManager.Inventories.Select(model => model.InventoryType);
			var inventoryUniqueTypes        = inventoryTypesEnums.Distinct();
			var uniqueCount                 = inventoryUniqueTypes.Count();
			if (uniqueCount != inventoryTypesModelsManager.Inventories.Count)
			{
				var distinctItems = inventoryTypesModelsManager
				                    .Inventories
				                    .GroupBy(model => model.InventoryType)
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

			var inventoryPacksModelsManager = Container.Resolve<InventoryPacksModelsSettings>();
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

		[UnityTest]
		public IEnumerator InventoryTypesContentDuplicatesTest()
		{
			PreInstall();
			PostInstall();

			var inventoryPacksModelsManager = Container.Resolve<InventoryPacksModelsSettings>();
			var inventoryUniqueTypes        = inventoryPacksModelsManager.Inventories.Select(model => model.InventoryType).Distinct();
			if (inventoryUniqueTypes.Count() != inventoryPacksModelsManager.Inventories.Count)
			{
				var distinctItems = inventoryPacksModelsManager
				                    .Inventories
				                    .GroupBy(model => model)
				                    .Where(g => g.Count() > 1)
				                    .SelectMany(r => r);

				Assert.Fail(string.Join("\r\n", distinctItems));
			}

			yield break;
		}
	}
}