using System;
using Factories;
using Inventories;

namespace Crafting.State
{
	internal class CancelCraftingTaskState : CraftingTaskState
	{
		public override CraftingTaskState OnTick(float         tickTime,
		                                         ref float     spentTime,
		                                         CraftingModel model,
		                                         IInventory    from,
		                                         IInventory    to,
		                                         Action        taskComplete)
		{
			
			foreach (var modelCraftingNeed in model.CraftingNeeds)
			{
				from.AddItems(modelCraftingNeed.model.Type, modelCraftingNeed.count, out int edge);
			}
			taskComplete?.Invoke();
			
			Factory.ReturnItem(this);
			return Factory.GetFactoryItem<EndCraftingTaskState>();
		}
	}
}