using System;
using System.Linq;
using Factories;
using Inventories;

namespace Crafting.State
{
	internal class InitializeCraftingTaskState : CraftingTaskState
	{
		public override CraftingTaskState OnTick(float         tickTime,
		                                         ref float     spentTime,
		                                         CraftingModel model,
		                                         IInventory    from,
		                                         IInventory    to,
		                                         Action        taskComplete)
		{
			if (model.CraftingNeeds.Any(craftingNeed => from.ItemsCount(craftingNeed.model.Type) < craftingNeed.count))
			{
				return this;
			}

			foreach (var craftingNeed in model.CraftingNeeds)
			{
				from.RemoveItem(craftingNeed.model.Type, craftingNeed.count);
			}

			Factory.ReturnItem(this);
			var state = Factory.GetFactoryItem<StartedCraftingTaskState>();
			return state.OnTick(tickTime, ref spentTime, model, from, to, taskComplete);
		}
	}
}