using UnityEngine;

namespace Inventories
{
	[CreateAssetMenu(fileName = "NewPackModel", menuName = "Models/Inventory/InventoryPack", order = 0)]
	public class InventoryPackModel : ScriptableObject
	{
		[SerializeField] private InventoryTypesEnum type        = InventoryTypesEnum.NOTHING;
		[SerializeField] private int                maxPackSize = 100;
		[SerializeField] private Sprite             icon;
		[SerializeField] private BuildingView       instance;

		public InventoryTypesEnum Type        => type;
		public int                MaxPackSize => maxPackSize;
		public Sprite             Icon        => icon;
		public BuildingView       Instance    => instance;

#if UNITY_INCLUDE_TESTS
		public static InventoryPackModel GetTestModel()
		{
			var inventoryPackModel = CreateInstance<InventoryPackModel>();
			inventoryPackModel.type        = InventoryTypesEnum.TEST_OBJECT;
			inventoryPackModel.maxPackSize = 50;
			inventoryPackModel.icon        = SpriteHelper.GetBlankSprite(128, 128);
			inventoryPackModel.instance    = new GameObject().AddComponent<BuildingView>();
			return inventoryPackModel;
		}

		public static InventoryPackModel GetNothingTestModel()
		{
			var inventoryPackModel = CreateInstance<InventoryPackModel>();
			inventoryPackModel.type        = InventoryTypesEnum.NOTHING;
			inventoryPackModel.maxPackSize = 50;
			inventoryPackModel.icon        = SpriteHelper.GetBlankSprite(128, 128);
			inventoryPackModel.instance    = new GameObject().AddComponent<BuildingView>();
			return inventoryPackModel;
		}
#endif
	}

	public enum InventoryTypesEnum
	{
		NOTHING,
		TEST_OBJECT,
		GENERATOR
	}
}