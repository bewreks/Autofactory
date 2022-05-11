using System;
using Inventories;
using UnityEngine;

namespace Crafting
{
	[CreateAssetMenu(fileName = "NewCraftingModel", menuName = "Models/Craft/CraftingModel", order = 0)]
	public class CraftingModel : ScriptableObject
	{
		[SerializeField] private CraftingNeed[] craftingNeeds;
		[SerializeField] private CraftingNeed   craftingResult;
		[SerializeField] private float          craftingTime;

		public CraftingNeed[] CraftingNeeds  => craftingNeeds;
		public CraftingNeed   CraftingResult => craftingResult;
		public float          CraftingTime   => craftingTime;

#if UNITY_INCLUDE_TESTS
		public static CraftingModel GetTestModel()
		{
			var craftingModel = CreateInstance<CraftingModel>();
			craftingModel.craftingTime = 1;
			craftingModel.craftingResult = new CraftingNeed
			                               {
				                               count = 1,
				                               model = InventoryPackModel.GetTestModel()
			                               };
			craftingModel.craftingNeeds = new[]
			                              {
				                              new CraftingNeed
				                              {
					                              count = 1,
					                              model = InventoryPackModel.GetNothingTestModel()
				                              }
			                              };
			return craftingModel;
		}

		public static CraftingModel GetZeroTestModel()
		{
			var craftingModel = CreateInstance<CraftingModel>();
			craftingModel.craftingTime = 0;
			craftingModel.craftingResult = new CraftingNeed
			                               {
				                               count = 1,
				                               model = InventoryPackModel.GetTestModel()
			                               };
			craftingModel.craftingNeeds = new[]
			                              {
				                              new CraftingNeed
				                              {
					                              count = 1,
					                              model = InventoryPackModel.GetNothingTestModel()
				                              }
			                              };
			return craftingModel;
		}

#endif
	}

	[Serializable]
	public class CraftingNeed
	{
		public InventoryPackModel model;
		public int                count;
	}
}