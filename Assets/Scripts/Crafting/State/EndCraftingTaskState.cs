using System;
using Inventories;

namespace Crafting.State
{
	internal class EndCraftingTaskState : CraftingTaskState
	{
		public override CraftingTaskState OnTick(float         tickTime,
		                                         ref float     spentTime,
		                                         CraftingModel model,
		                                         IInventory    from,
		                                         IInventory    to,
		                                         Action        taskComplete)
		{
			return this;
		}
	}
}