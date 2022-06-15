using Windows.CraftingWindow;
using Windows.InventoryWindow;
using Factories;
using Installers;
using Instantiate;
using Inventories;
using Players;
using Players.Interfaces;
using UnityEngine;
using Zenject;

namespace Game.States
{
	public class NormalGameState : IGameState
	{
		[Inject] private InstantiateManager    _instantiateManager;
		[Inject] private DiContainer           _diContainer;
		[Inject] private WindowsManager        _windowsManager;
		[Inject] private IPlayerInputController _playerInputController;

		public IGameState OnUpdate(GameModel model, DiContainer container)
		{
			if (_playerInputController.Player.FastUsing1.WasPressedThisFrame())
			{
				return PlayerInputHelper.TryToInstantiate(InventoryObjectsTypesEnum.BASE_ELECTRIC_POLE,
				                                          model,
				                                          this,
				                                          _instantiateManager,
				                                          _diContainer,
				                                          _playerInputController);
			}

			if (_playerInputController.Player.FastUsing2.WasPressedThisFrame())
			{
				return PlayerInputHelper.TryToInstantiate(InventoryObjectsTypesEnum.GENERATOR,
				                                          model,
				                                          this,
				                                          _instantiateManager,
				                                          _diContainer,
				                                          _playerInputController);
			}

			if (_playerInputController.Player.FastUsing3.WasPressedThisFrame())
			{
				return PlayerInputHelper.TryToInstantiate(InventoryObjectsTypesEnum.TEST_OBJECT,
				                                          model,
				                                          this,
				                                          _instantiateManager,
				                                          _diContainer,
				                                          _playerInputController);
			}

			if (_playerInputController.Player.InventoryWindows.WasPressedThisFrame())
			{
				_windowsManager.OpenWindow<InventoryWindow>();
				Factory.ReturnItem(this);
				return Factory.GetFactoryItem<WindowGameState>(_diContainer);
			}

			if (_playerInputController.Player.CraftWindows.WasPressedThisFrame())
			{
				_windowsManager.OpenWindow<CraftingWindow>();
				Factory.ReturnItem(this);
				return Factory.GetFactoryItem<WindowGameState>(_diContainer);
			}

			return this;
		}

		public void OnFixedUpdate(GameModel model)
		{
		}
	}
}