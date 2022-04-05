using System;
using Windows;
using Inventory;
using UniRx;
using UnityEngine;
using UnityEngine.Assertions;
using Zenject;
using Object = UnityEngine.Object;

namespace Installers
{
	public class SampleInstaller : MonoInstaller<SampleInstaller>
	{
		[SerializeField] private LayerMask _groundMask;

		public override void InstallBindings()
		{
			Observable.OnceApplicationQuit().Subscribe(unit => { _disposables.Dispose(); }).AddTo(_disposables);


			Container.Bind<GameModel>().FromNew().AsSingle();
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
			_disposables.Add(inventory);
			_disposables.Add(instantiateManager);

			Assert.IsTrue(inventory.AddItem(InventoryTypesEnum.TEST_OBJECT));
			Container.Resolve<GameModel>().SelectedPack = inventory.GetPack(InventoryTypesEnum.TEST_OBJECT);
			// var windowsManager = Container.Resolve<WindowsManager>();
			// windowsManager.OpenWindow<InventoryWindow>();
		}
	}

	public class InstantiateManager : IDisposable
	{
		[Inject] private GameModel   _gameModel;
		[Inject] private DiContainer _container;
		[Inject] private LayerMask   _groundMask;

		private CompositeDisposable _disposable = new CompositeDisposable();

		[Inject]
		public void Construct()
		{
			Observable.EveryUpdate().Subscribe(l =>
			{
				// TODO: Add state machine
				if (Input.GetMouseButtonUp(0) && _gameModel.InstantiablePack != null)
				{
					_gameModel.InstantiablePack = null;
					_gameModel.SelectedPack     = null;
				}

				if (Input.GetMouseButtonUp(0) && _gameModel.SelectedPack != null)
				{
					_gameModel.InstantiablePack =
						_container.InstantiatePrefab(_gameModel.SelectedPack.InstancePrefab);
				}

				if (Input.GetKeyUp(KeyCode.Escape) && _gameModel.InstantiablePack != null)
				{
					Object.Destroy(_gameModel.InstantiablePack);
					_gameModel.InstantiablePack = null;
					_gameModel.SelectedPack     = null;
				}

				if (_gameModel.InstantiablePack != null)
				{
					if (!(Camera.main is null))
					{
						var ray = Camera.main.ScreenPointToRay(Input.mousePosition);

						if (Physics.Raycast(ray, out var hit, float.MaxValue, _groundMask))
						{
							var hitInfoPoint = hit.point;
							if (hit.collider.CompareTag("Ground"))
							{
								hitInfoPoint.x = SetStep(hit.point.x);
								hitInfoPoint.z = SetStep(hit.point.z);

								_gameModel.InstantiablePack.transform.position = hitInfoPoint;
							}

							Debug.DrawLine(ray.origin, hit.point, Color.red);
						}
					}
				}
			}).AddTo(_disposable);
		}

		private float SetStep(float coordX)
		{
			return (int)(coordX / 0.5) * 0.5f;
		}

		public void Dispose()
		{
			_disposable?.Dispose();
		}
	}

	public class GameModel
	{
		public InventoryPack SelectedPack;
		public GameObject    InstantiablePack;
	}
}