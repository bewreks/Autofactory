using Factories;
using Instantiate;
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
		
		public IGameState OnUpdate(GameModel model, DiContainer container)
		{
			if (Input.GetKeyUp(KeyCode.Escape))
			{
				Object.Destroy(model.InstantiablePack);
				
				model.SelectedPack     = null;
				model.InstantiablePack = null;
				Factory.ReturnItem(this);
				return Factory.GetFactoryItem<NormalGameState>(_diContainer);
			}

			if (Input.GetMouseButtonUp(0))
			{
				_instantiateManager.InstantiateFinal();
				Object.Destroy(model.InstantiablePack);
				
				model.SelectedPack     = null;
				model.InstantiablePack = null;
				Factory.ReturnItem(this);
				return Factory.GetFactoryItem<NormalGameState>(_diContainer);
			}
			
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

			return this;
		}

		public void OnFixedUpdate(GameModel model)
		{
			_instantiateManager.UpdatePreviewPosition(model.MousePosition);
			_gameController.RotatePlayerTo(model.MousePosition);
		}
	}
}