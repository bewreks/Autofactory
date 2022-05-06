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
	}

	[Serializable]
	public class CraftingNeed
	{
		public InventoryPackModel model;
		public int                count;
	}
}