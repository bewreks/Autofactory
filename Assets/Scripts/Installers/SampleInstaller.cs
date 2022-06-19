using Windows;
using Crafting;
using Electricity;
using Game;
using Instantiate;
using Inventories;
using Players;
using Players.Interfaces;
using UniRx;
using Zenject;

namespace Installers
{
	public class SampleInstaller : MonoInstaller<SampleInstaller>
	{
		public override void InstallBindings()
		{
			Observable.OnceApplicationQuit().Subscribe(unit => { _disposables.Dispose(); }).AddTo(_disposables);

			Container.BindInterfacesTo<PlayerInputController>().FromNew().AsSingle();
			Container.Bind<GameController>().FromNew().AsSingle();
			Container.Bind<CraftingController>().FromNew().AsSingle();
			Container.BindInterfacesTo<ElectricityController>().FromNew().AsSingle();
			Container.Bind<WindowsManager>().FromNew().AsSingle();
			Container.Bind<InstantiateManager>().FromNew().AsSingle();
		}

		private CompositeDisposable _disposables = new CompositeDisposable();

		public override void Start()
		{
			var inputController       = Container.Resolve<IPlayerInputController>();
			var gameController        = Container.Resolve<GameController>();
			var instantiateManager    = Container.Resolve<InstantiateManager>();
			var craftingController    = Container.Resolve<CraftingController>();
			var electricityController = Container.Resolve<IElectricityController>();
			var windowsManager        = Container.Resolve<WindowsManager>();
			_disposables.Add(inputController);
			_disposables.Add(gameController);
			_disposables.Add(instantiateManager);
			_disposables.Add(craftingController);
			_disposables.Add(electricityController);
			_disposables.Add(windowsManager);

			var gameModel = Container.Resolve<IGameModel>();
			gameModel.PlayerModel.Inventory.AddItems(InventoryObjectsTypesEnum.BASE_ELECTRIC_POLE, 10, out var edge);
			gameModel.PlayerModel.Inventory.AddItems(InventoryObjectsTypesEnum.GENERATOR,          10, out edge);
			gameModel.PlayerModel.Inventory.AddItems(InventoryObjectsTypesEnum.TEST_OBJECT,        1,  out edge);
		}
	}
}