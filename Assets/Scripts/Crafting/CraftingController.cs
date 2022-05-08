using System;
using System.Collections.Generic;
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

		private List<CraftingTask> _tasks = new List<CraftingTask>();

		[Inject]
		public void Construct()
		{
			const float timerTimeConst = 0.1f;
			Observable.Timer(TimeSpan.FromSeconds(timerTimeConst)).Repeat().Subscribe(l =>
			{
				_tasks.ForEach(task => task.Tick(timerTimeConst));
			}).AddTo(_disposables);
		}

		public void StartCraft(IInventory from, IInventory to, InventoryObjectsTypesEnum types)
		{
			var craftingTask = Factory.GetFactoryItem<CraftingTask>();
			craftingTask.Initialize(from, to, _craftSettings.GetModel(types));
			craftingTask.TaskComplete += task =>
			{
				_tasks.Remove(task);
				task.Dispose();
				Factory.ReturnItem(task);
			};
			_tasks.Add(craftingTask);
		}

		public void Dispose()
		{
			_disposables.Dispose();
		}
	}
}