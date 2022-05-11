using UnityEngine;

namespace Inventories
{
	[CreateAssetMenu(fileName = "NewPackModel", menuName = "Models/Inventory/InventoryPack", order = 0)]
	public class InventoryPackModel : ScriptableObject
	{
		[SerializeField] private InventoryObjectsTypesEnum type        = InventoryObjectsTypesEnum.NOT_CONFIGURED;
		[SerializeField] private int                       maxPackSize = 100;
		[SerializeField] private Sprite                    icon;
		[SerializeField] private BuildingView              instance;

		public InventoryObjectsTypesEnum Type        => type;
		public int                MaxPackSize => maxPackSize;
		public Sprite             Icon        => icon;
		public BuildingView       Instance    => instance;

#if UNITY_INCLUDE_TESTS
		public static InventoryPackModel GetTestModel()
		{
			var inventoryPackModel = CreateInstance<InventoryPackModel>();
			inventoryPackModel.type        = InventoryObjectsTypesEnum.TEST_OBJECT;
			inventoryPackModel.maxPackSize = 50;
			inventoryPackModel.icon        = SpriteHelper.GetBlankSprite(128, 128);
			inventoryPackModel.instance    = Resources.Load<BuildingView>("InstanceModels/TestObjectModel");
			return inventoryPackModel;
		}

		public static InventoryPackModel GetNothingTestModel()
		{
			var inventoryPackModel = CreateInstance<InventoryPackModel>();
			inventoryPackModel.type        = InventoryObjectsTypesEnum.NOTHING;
			inventoryPackModel.maxPackSize = 50;
			inventoryPackModel.icon        = SpriteHelper.GetBlankSprite(128, 128);
			inventoryPackModel.instance    = Resources.Load<BuildingView>("InstanceModels/TestObjectModel");
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
	}
}