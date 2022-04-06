using Factories;
using Instantiate;
using UnityEngine;
using Zenject;

namespace Game.States
{
	public class SelectedItemGameState : IGameState
	{
		[Inject] private InstantiateManager _instantiateManager;
		[Inject] private DiContainer        _diContainer;
		
		public IGameState OnUpdate(GameModel _model, DiContainer _container)
		{
			if (Input.GetKeyUp(KeyCode.Escape))
			{
				Object.Destroy(_model.InstantiablePack);
				
				_model.SelectedPack     = null;
				_model.InstantiablePack = null;
				Factory.ReturnItem(this);
				return Factory.GetFactoryItem<NormalGameState>(_diContainer);
			}

			if (Input.GetMouseButtonUp(0))
			{
				_instantiateManager.InstantiateFinal();
				Object.Destroy(_model.InstantiablePack);
				
				_model.SelectedPack     = null;
				_model.InstantiablePack = null;
				Factory.ReturnItem(this);
				return Factory.GetFactoryItem<NormalGameState>(_diContainer);
			}

			if (_model.InstantiablePack != null)
			{
				if (!(Camera.main is null))
				{
					var ray = Camera.main.ScreenPointToRay(Input.mousePosition);

					var groundMask = _container.Resolve<LayerMask>();

					if (Physics.Raycast(ray, out var hit, float.MaxValue, groundMask))
					{
						if (hit.collider.CompareTag("Ground"))
						{
							_instantiateManager.UpdatePreviewPosition(hit.point);
						}

						Debug.DrawLine(ray.origin, hit.point, Color.red);
					}
				}
			}

			return this;
		}
	}
}