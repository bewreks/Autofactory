using Factories;
using Instantiate;
using Inventories;
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
				var ray = Camera.main.ScreenPointToRay(Input.mousePosition);

				var groundMask = _gameSettings.GroundMask;

				if (Physics.Raycast(ray, out var hit, float.MaxValue, groundMask))
				{
					if (hit.collider.CompareTag("Ground"))
					{
						model.MousePosition = hit.point;
					}

					Debug.DrawLine(ray.origin, hit.point, Color.red);
				}
			}
			
			if (Input.GetKeyUp(KeyCode.Alpha1))
			{
				var inventory = model.PlayerModel.Inventory;
				model.SelectedPack     = inventory.GetPacks(InventoryTypesEnum.TEST_OBJECT);
				model.InstantiablePack = _instantiateManager.InstantiatePreview();
				Factory.ReturnItem(this);
				return Factory.GetFactoryItem<SelectedItemGameState>(_diContainer);
			}

			return this;
		}

		public void OnFixedUpdate(GameModel model)
		{
			_gameController.RotatePlayerTo(model.MousePosition);
		}
	}
}