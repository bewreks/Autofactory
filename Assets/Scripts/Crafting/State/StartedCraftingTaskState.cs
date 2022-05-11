using System;
using Factories;
using Inventories;
using UnityEngine;

namespace Crafting.State
{
	internal class StartedCraftingTaskState : CraftingTaskState
	{
		public override CraftingTaskState OnTick(float         tickTime,
		                                         ref float     spentTime,
		                                         CraftingModel model,
		                                         IInventory    from,
		                                         IInventory    to,
		                                         Action        taskComplete)
		{
			spentTime += tickTime;
			if (spentTime >= model.CraftingTime)
			{
				if (to.AddItems(model.CraftingResult.model.Type, model.CraftingResult.count, out int edge))
				{
					Debug.Log("CRAFTING COMPLETE");
				}
				else
				{
					Debug.Log("CRAFTING FAIL");
				}
				taskComplete?.Invoke();
				
				Factory.ReturnItem(this);
				return Factory.GetFactoryItem<EndCraftingTaskState>();
			}

			return this;
		}
	}
}