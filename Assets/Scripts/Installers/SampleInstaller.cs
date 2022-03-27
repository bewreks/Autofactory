using Windows;
using Windows.InventoryWindow;
using Inventory;
using UniRx;
using UnityEngine.Assertions;
using Zenject;

namespace Installers
{
	public class SampleInstaller : MonoInstaller<SampleInstaller>
	{
		
		private CompositeDisposable _disposables = new CompositeDisposable();
		
		public override void InstallBindings()
		{
			Observable.OnceApplicationQuit().Subscribe(unit =>
			{
				_disposables.Dispose();
			}).AddTo(_disposables);

			
			Container.BindInterfacesTo<Inventory.Inventory>().AsSingle();
			Container.Bind<WindowsManager>().FromNew().AsSingle();
		}

		public override void Start()
		{
			var inventory = Container.Resolve<IInventory>();
			_disposables.Add(inventory);

			Assert.IsTrue(inventory.AddItem(InventoryTypesEnum.GENERATOR));
			// Assert.IsFalse(inventory.RemoveItem(InventoryTypesEnum.GENERATOR));
			var windowsManager = Container.Resolve<WindowsManager>();
			windowsManager.OpenWindow<InventoryWindow>();
			Assert.IsTrue(inventory.RemoveItem(InventoryTypesEnum.GENERATOR));
		}
	}
}