using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Factories;
using Installers;
using Inventories;
using ModestTree;
using UniRx;
using Zenject;

namespace Crafting
{
	public class CraftingController : IDisposable
	{
		[Inject] private CraftSettings _craftSettings;

		private CompositeDisposable _disposables = new CompositeDisposable();

		private Dictionary<(IInventory, IInventory), Queue<CraftingTask>> _craftingMap =
			new Dictionary<(IInventory, IInventory), Queue<CraftingTask>>();

		private List<(IInventory, IInventory)> _endedTasks = new List<(IInventory, IInventory)>();

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
				var selectMany = _craftingMap.Select(pair => pair.Value.Peek());
				var parallelLoopResult = Parallel.ForEach(selectMany, parallelOptions, task => { task.Tick(timerTimeConst); });
				// _tasks.ForEach(task => task.Tick(timerTimeConst));
				if (parallelLoopResult.IsCompleted)
				{
					_endedTasks.ForEach(task =>
					{
						if (_craftingMap.TryGetValue(task, out var queue))
						{
							queue.Dequeue();
							if (queue.IsEmpty())
							{
								_craftingMap.Remove(task);
							}
						}
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
				_endedTasks.Add((from, to));
				task.Dispose();
				Factory.ReturnItem(task);
			};
			if (!_craftingMap.TryGetValue((from, to), out var queue))
			{
				queue = new Queue<CraftingTask>();
				_craftingMap.Add((from, to), queue);
			}
			queue.Enqueue(craftingTask);
		}

		public void CancelCraft(IInventory from, IInventory to)
		{
			if (_craftingMap.TryGetValue((from, to), out var queue))
			{
				var craftingTasks = queue.ToArray();
				foreach (var task in craftingTasks)
				{
					if (!task?.IsComplete??false)
					{
						task.Cancel();
					}
				}
			}
		}

		public void Dispose()
		{
			_disposables.Dispose();
		}
	}
}