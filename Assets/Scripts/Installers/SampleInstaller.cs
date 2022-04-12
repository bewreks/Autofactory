using Windows;
using Game;
using Instantiate;
using Inventory;
using UniRx;
using UnityEngine;
using UnityEngine.Assertions;
using Zenject;

namespace Installers
{
	public class SampleInstaller : MonoInstaller<SampleInstaller>
	{
		[SerializeField] private LayerMask _groundMask;

		public override void InstallBindings()
		{
			Observable.OnceApplicationQuit().Subscribe(unit => { _disposables.Dispose(); }).AddTo(_disposables);

			Container.BindInterfacesTo<GameModel>().AsSingle();
			Container.Bind<GameController>().FromNew().AsSingle();
			Container.BindInterfacesTo<Inventory.Inventory>().AsSingle();
			Container.Bind<WindowsManager>().FromNew().AsSingle();
			Container.Bind<InstantiateManager>().FromNew().AsSingle();
			Container.Bind<LayerMask>().FromInstance(_groundMask).AsSingle();
		}

		private CompositeDisposable _disposables = new CompositeDisposable();

		public override void Start()
		{
			var inventory          = Container.Resolve<IInventory>();
			var instantiateManager = Container.Resolve<InstantiateManager>();
			var gameController     = Container.Resolve<GameController>();
			_disposables.Add(inventory);
			_disposables.Add(gameController);
			_disposables.Add(instantiateManager);

			Assert.IsTrue(inventory.AddItems(InventoryTypesEnum.TEST_OBJECT));
			// var windowsManager = Container.Resolve<WindowsManager>();
			// windowsManager.OpenWindow<InventoryWindow>();
		}
	}
}