using Buildings.Models;
using Buildings.Views;
using Helpers;
using UnityEngine;

namespace Inventories
{
	[CreateAssetMenu(fileName = "NewPackModel", menuName = "Models/Inventory/InventoryPacks/Base", order = 0)]
	public class InventoryPackModel : ScriptableObject
	{
		[SerializeField] private InventoryObjectsTypesEnum type        = InventoryObjectsTypesEnum.NOT_CONFIGURED;
		[SerializeField] private int                       maxPackSize = 100;
		[SerializeField] private Sprite                    icon;
		[SerializeField] private BuildingModel             buildingModel;

		public InventoryObjectsTypesEnum Type          => type;
		public int                       MaxPackSize   => maxPackSize;
		public Sprite                    Icon          => icon;
		public BuildingModel             BuildingModel => buildingModel;

#if UNITY_INCLUDE_TESTS
		public static InventoryPackModel GetTestModel()
		{
			var inventoryPackModel = CreateInstance<InventoryPackModel>();
			inventoryPackModel.type        = InventoryObjectsTypesEnum.TEST_OBJECT;
			inventoryPackModel.maxPackSize = 50;
			inventoryPackModel.icon        = SpriteHelper.GetBlankSprite(128, 128);
			return inventoryPackModel;
		}

		public static InventoryPackModel GetNothingTestModel()
		{
			var inventoryPackModel = CreateInstance<InventoryPackModel>();
			inventoryPackModel.type        = InventoryObjectsTypesEnum.NOTHING;
			inventoryPackModel.maxPackSize = 50;
			inventoryPackModel.icon        = SpriteHelper.GetBlankSprite(128, 128);
			return inventoryPackModel;
		}
#endif
	}

	public enum InventoryObjectsTypesEnum
	{
		NOTHING,
		TEST_OBJECT,
		NOT_CONFIGURED,
		GENERATOR,
		BASE_ELECTRIC_POLE,
		ELECTRICAL_BUILDING
	}
}