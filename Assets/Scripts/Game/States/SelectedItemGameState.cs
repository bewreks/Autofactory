using Factories;
using Installers;
using Instantiate;
using Players;
using UnityEngine;
using Zenject;

namespace Game.States
{
	public class SelectedItemGameState : IGameState
	{
		[Inject] private InstantiateManager _instantiateManager;
		[Inject] private GameSettings       _gameSettings;
		[Inject] private DiContainer        _diContainer;
		[Inject] private GameController     _gameController;
		[Inject] private PlayerInputActions _playerInputActions;

		public IGameState OnUpdate(GameModel model, DiContainer container)
		{
			if (Input.GetKeyUp(KeyCode.Escape))
			{
				Object.Destroy(model.InstantiablePack.gameObject);

				model.SelectedPack     = null;
				model.InstantiablePack = null;
				Factory.ReturnItem(this);
				return Factory.GetFactoryItem<NormalGameState>(_diContainer);
			}

			if (Input.GetMouseButtonUp(0) && !model.InstantiablePack.Triggered)
			{
				_instantiateManager.InstantiateFinal();

				model.SelectedPack     = null;
				model.InstantiablePack = null;
				Factory.ReturnItem(this);
				return Factory.GetFactoryItem<NormalGameState>(_diContainer);
			}

			if (!(Camera.main is null))
			{
				if (PlayerInputHelper.GetWorldMousePosition(_gameSettings.GroundMask, Camera.main,
				                                            _playerInputActions, out var mousePosition))
				{
					model.MousePosition = mousePosition;
				}

				var deltaTime    = Time.deltaTime;
				var currentInput = PlayerInputHelper.GetPlayerInput(Camera.main, deltaTime, _playerInputActions);
				model.MoveDelta += currentInput;
			}

			return this;
		}

		public void OnFixedUpdate(GameModel model)
		{
			_instantiateManager.UpdatePreviewPosition(model.MousePosition);
			_gameController.RotatePlayerTo(model.MousePosition);
			_gameController.MovePlayer(model.MoveDelta);
			model.MoveDelta = Vector3.zero;
		}
	}
}