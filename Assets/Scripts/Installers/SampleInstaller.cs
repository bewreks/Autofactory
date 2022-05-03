using Windows;
using Windows.InventoryWindow;
using Game;
using Instantiate;
using Inventories;
using UniRx;
using UnityEngine;
using UnityEngine.Assertions;
using Zenject;

namespace Installers
{
	public class SampleInstaller : MonoInstaller<SampleInstaller>
	{
		public override void InstallBindings()
		{
			Observable.OnceApplicationQuit().Subscribe(unit => { _disposables.Dispose(); }).AddTo(_disposables);

			Container.Bind<GameController>().FromNew().AsSingle();
			Container.Bind<WindowsManager>().FromNew().AsSingle();
			Container.Bind<InstantiateManager>().FromNew().AsSingle();
		}

		private CompositeDisposable _disposables = new CompositeDisposable();

		public override void Start()
		{
			var gameController     = Container.Resolve<GameController>();
			var gameModel          = Container.Resolve<IGameModel>();
			var instantiateManager = Container.Resolve<InstantiateManager>();
			_disposables.Add(gameController);
			_disposables.Add(instantiateManager);

			gameModel.PlayerModel.Inventory.AddItems(InventoryTypesEnum.TEST_OBJECT);
			// var windowsManager = Container.Resolve<WindowsManager>();
			// windowsManager.OpenWindow<InventoryWindow>();
		}
	}
}