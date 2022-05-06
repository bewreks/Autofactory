using System;
using Inventories;

namespace Crafting.State
{
	internal abstract class CraftingTaskState
	{
		public abstract CraftingTaskState OnTick(float         tickTime,
		                                         ref float     spentTime,
		                                         CraftingModel model,
		                                         IInventory    from,
		                                         IInventory    to,
		                                         Action        taskComplete);
	}
}