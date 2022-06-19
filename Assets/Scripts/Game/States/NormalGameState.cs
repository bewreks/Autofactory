using Windows;
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
			if (_playerInputController.UsingBeforeActions.FastUsing1.WasPressedThisFrame())
			{
				return PlayerInputHelper.TryToInstantiate(InventoryObjectsTypesEnum.BASE_ELECTRIC_POLE,
				                                          model,
				                                          this,
				                                          _instantiateManager,
				                                          _diContainer,
				                                          _playerInputController);
			}

			if (_playerInputController.UsingBeforeActions.FastUsing2.WasPressedThisFrame())
			{
				return PlayerInputHelper.TryToInstantiate(InventoryObjectsTypesEnum.GENERATOR,
				                                          model,
				                                          this,
				                                          _instantiateManager,
				                                          _diContainer,
				                                          _playerInputController);
			}

			if (_playerInputController.UsingBeforeActions.FastUsing3.WasPressedThisFrame())
			{
				return PlayerInputHelper.TryToInstantiate(InventoryObjectsTypesEnum.TEST_OBJECT,
				                                          model,
				                                          this,
				                                          _instantiateManager,
				                                          _diContainer,
				                                          _playerInputController);
			}

			if (_playerInputController.WindowsActions.InventoryWindows.WasPressedThisFrame())
			{
				if (_windowsManager.IsOpened<InventoryWindow>())
				{
					_windowsManager.CloseWindow<InventoryWindow>();
				}
				else
				{
					_windowsManager.OpenWindow<InventoryWindow>(IWindowManager.WindowOpenOption.Unique);
				}
			}

			if (_playerInputController.WindowsActions.CraftWindows.WasPressedThisFrame())
			{
				if (_windowsManager.IsOpened<CraftingWindow>())
				{
					_windowsManager.CloseWindow<CraftingWindow>();
				}
				else
				{
					_windowsManager.OpenWindow<CraftingWindow>(IWindowManager.WindowOpenOption.Unique);
				}
			}

			return this;
		}

		public void OnFixedUpdate(GameModel model)
		{
		}
	}
}