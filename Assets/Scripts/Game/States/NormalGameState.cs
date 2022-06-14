using Windows.CraftingWindow;
using Windows.InventoryWindow;
using Factories;
using Installers;
using Instantiate;
using Inventories;
using Players;
using UnityEngine;
using Zenject;

namespace Game.States
{
	public class NormalGameState : IGameState
	{
		[Inject] private InstantiateManager _instantiateManager;
		[Inject] private DiContainer        _diContainer;
		[Inject] private GameSettings       _gameSettings;
		[Inject] private GameController     _gameController;
		[Inject] private WindowsManager     _windowsManager;

		public IGameState OnUpdate(GameModel model, DiContainer container)
		{
			if (!(Camera.main is null))
			{
				if (PlayerInputHelper.GetWorldMousePosition(_gameSettings.GroundMask, Camera.main,
				                                            out var mousePosition))
				{
					model.MousePosition = mousePosition;
				}

				var deltaTime    = Time.deltaTime;
				var currentInput = PlayerInputHelper.GetPlayerInput(Camera.main, deltaTime);
				model.MoveDelta += currentInput;
			}

/*			if (Input.GetKeyUp(KeyCode.Alpha1))
			{
				return PlayerInputHelper.TryToInstantiate(InventoryObjectsTypesEnum.BASE_ELECTRIC_POLE,
				                                          model,
				                                          this,
				                                          _instantiateManager,
				                                          _diContainer);
			}

			if (Input.GetKeyUp(KeyCode.Alpha2))
			{
				return PlayerInputHelper.TryToInstantiate(InventoryObjectsTypesEnum.GENERATOR,
				                                          model,
				                                          this,
				                                          _instantiateManager,
				                                          _diContainer);
			}

			if (Input.GetKeyUp(KeyCode.Alpha3))
			{
				return PlayerInputHelper.TryToInstantiate(InventoryObjectsTypesEnum.TEST_OBJECT,
				                                          model,
				                                          this,
				                                          _instantiateManager,
				                                          _diContainer);
			}

			if (Input.GetKeyUp(KeyCode.Tab))
			{
				_windowsManager.OpenWindow<InventoryWindow>();
			}

			if (Input.GetKeyUp(KeyCode.E))
			{
				_windowsManager.OpenWindow<CraftingWindow>();
			}*/

			return this;
		}

		public void OnFixedUpdate(GameModel model)
		{
			_gameController.RotatePlayerTo(model.MousePosition);
			_gameController.MovePlayer(model.MoveDelta);
			model.MoveDelta = Vector3.zero;
		}
	}
}