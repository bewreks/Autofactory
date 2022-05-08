using UnityEngine;

namespace Inventories
{
	[CreateAssetMenu(fileName = "NewInventoryTypeModel", menuName = "Models/Inventory/InventoryType", order = 3)]
	public class InventoryTypesModel : ScriptableObject
	{
		[SerializeField] private InventoryTypesEnum inventoryType;

		[SerializeField]
		private int limit;

		public int               Limit         => limit;
		public InventoryTypesEnum InventoryType => inventoryType;

#if UNITY_INCLUDE_TESTS
		public static InventoryTypesModel GetUnlimitedTestModel()
		{
			var inventoryPackModel = CreateInstance<InventoryTypesModel>();
			inventoryPackModel.limit         = int.MaxValue;
			inventoryPackModel.inventoryType = InventoryTypesEnum.UNLIMITED;
			return inventoryPackModel;
		}

		public static InventoryTypesModel GetTestModel()
		{
			var inventoryPackModel = CreateInstance<InventoryTypesModel>();
			inventoryPackModel.limit         = 1;
			inventoryPackModel.inventoryType = InventoryTypesEnum.TEST;
			return inventoryPackModel;
		}
#endif
	}

	public enum InventoryTypesEnum
	{
		TEST,
		UNLIMITED,
		WOOD_BOX
	}
}