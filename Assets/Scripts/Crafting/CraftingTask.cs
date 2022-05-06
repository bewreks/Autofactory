using System;
using Crafting.State;
using Factories;
using Inventories;

namespace Crafting
{
	public class CraftingTask : IDisposable
	{
		private IInventory    _from;
		private IInventory    _to;
		private CraftingModel _model;

		private float             _spentTime;
		private CraftingTaskState _state;

		public event Action<CraftingTask> TaskComplete;

		public void Initialize(IInventory from, IInventory to, CraftingModel craftingModel)
		{
			_from  = from;
			_to    = to;
			_model = craftingModel;
			_state = Factory.GetFactoryItem<InitializeCraftingTaskState>();
		}

		public void Tick(float tickTime)
		{
			_state = _state?.OnTick(tickTime, ref _spentTime, _model, _from, _to,
			                        () => { TaskComplete?.Invoke(this); });
		}

		public void Dispose()
		{
			Factory.ReturnItem(_state);
			_spentTime   = 0;
			_from        = null;
			_to          = null;
			_model       = null;
			TaskComplete = null;
			_state       = null;
		}
	}
}