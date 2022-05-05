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

		public IGameState OnUpdate(GameModel model, DiContainer container)
		{
			if (!(Camera.main is null))
			{
				if (PlayerInputHelper.GetWorldMousePosition(_gameSettings.GroundMask, Camera.main, out var mousePosition))
				{
					model.MousePosition = mousePosition;
				}
				var deltaTime    = Time.deltaTime;
				var currentInput = PlayerInputHelper.GetPlayerInput(Camera.main, deltaTime);
				model.MoveDelta += currentInput;
			}
			
			if (Input.GetKeyUp(KeyCode.Alpha1))
			{
				var inventory = model.PlayerModel.Inventory;
				model.SelectedPack     = inventory.GetPacks(InventoryObjectsTypesEnum.TEST_OBJECT);
				model.InstantiablePack = _instantiateManager.InstantiatePreview();
				Factory.ReturnItem(this);
				return Factory.GetFactoryItem<SelectedItemGameState>(_diContainer);
			}

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