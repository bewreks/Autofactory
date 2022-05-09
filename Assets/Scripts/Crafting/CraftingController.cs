using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Factories;
using Installers;
using Inventories;
using UniRx;
using Zenject;

namespace Crafting
{
	public class CraftingController : IDisposable
	{
		[Inject] private CraftSettings _craftSettings;

		private CompositeDisposable _disposables = new CompositeDisposable();

		private List<CraftingTask> _tasks      = new List<CraftingTask>();
		private List<CraftingTask> _endedTasks = new List<CraftingTask>();

		[Inject]
		public void Construct()
		{
			const float timerTimeConst = 0.1f;
			Observable.Timer(TimeSpan.FromSeconds(timerTimeConst)).Repeat().Subscribe(l =>
			{
				var parallelOptions = new ParallelOptions
				{
					                      MaxDegreeOfParallelism = 5
				};
				var parallelLoopResult = Parallel.ForEach(_tasks, parallelOptions, task => { task.Tick(timerTimeConst); });
				// _tasks.ForEach(task => task.Tick(timerTimeConst));
				if (parallelLoopResult.IsCompleted)
				{
					_endedTasks.ForEach(task =>
					{
						_tasks.Remove(task);
					});
					_endedTasks.Clear();
				}
			}).AddTo(_disposables);
		}

		public void StartCraft(IInventory from, IInventory to, InventoryObjectsTypesEnum types)
		{
			var craftingTask = Factory.GetFactoryItem<CraftingTask>();
			craftingTask.Initialize(from, to, _craftSettings.GetModel(types));
			craftingTask.TaskComplete += task =>
			{
				_endedTasks.Add(task);
				task.Dispose();
				Factory.ReturnItem(task);
			};
			_tasks.Add(craftingTask);
		}

		public void CancelCraft(IInventory from, IInventory to)
		{
			var task = _tasks.FirstOrDefault(task => task.IsSame(from, to));
			if (!task?.IsComplete??false)
			{
				task.Cancel();
			}
		}

		public void Dispose()
		{
			_disposables.Dispose();
		}
	}
}