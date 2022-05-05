using UnityEngine;

namespace Inventories
{
	[CreateAssetMenu(fileName = "NewInventoryTypeModel", menuName = "Models/Inventory/InventoryType", order = 3)]
	public class InventoryTypesModel : ScriptableObject
	{
		[SerializeField] private InventoryTypesEnum inventoryType;

		[SerializeField, Range(0, ushort.MaxValue)]
		private ushort limit;

		public        ushort              Limit         => limit;
		public        InventoryTypesEnum  InventoryType => inventoryType;

#if UNITY_INCLUDE_TESTS
		public static InventoryTypesModel GetUnlimitedTestModel()
		{
			var inventoryPackModel = CreateInstance<InventoryTypesModel>();
			inventoryPackModel.limit         = ushort.MaxValue;
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