using Factories;
using Game;
using Game.States;
using Instantiate;
using Inventories;
using Players.Interfaces;
using UnityEngine;
using Zenject;

namespace Players
{
	public static class PlayerInputHelper
	{
		public static bool GetWorldMousePosition(LayerMask              groundMask,
		                                         Camera                 castCamera,
		                                         IPlayerInputController playerInputController,
		                                         out Vector3            mousePosition)
		{
			mousePosition = Vector3.zero;

			var pos = playerInputController.MousePosition;
			var ray = castCamera.ScreenPointToRay(pos);

			if (Physics.Raycast(ray, out var hit, float.MaxValue, groundMask))
			{
				Debug.DrawLine(ray.origin, hit.point, Color.red);
				{
					mousePosition = hit.point;
					return true;
				}
			}

			return false;
		}

		public static Vector3 GetPlayerInput(Camera camera, float delta, IPlayerInputController playerInputController)
		{
			var transform = camera.transform;
			var forward   = transform.forward;
			var right     = transform.right;

			forward.y = 0;
			right.y   = 0;
			forward.Normalize();
			right.Normalize();

			var movement = playerInputController.MovementDirection;

			right   *= movement.x;
			forward *= movement.y;

			forward += right;

			forward.Normalize();

			return forward * delta;
		}

		public static IGameState TryToInstantiate(InventoryObjectsTypesEnum type,
		                                          GameModel                 model,
		                                          IGameState                gameState,
		                                          InstantiateManager        instantiateManager,
		                                          DiContainer               diContainer,
		                                          IPlayerInputController    playerInputController)
		{
			var inventory = model.PlayerModel.Inventory;
			if (inventory.ItemsCount(type) > 0)
			{
				model.SelectedPack     = inventory.GetPacks(type);
				model.InstantiablePack = instantiateManager.InstantiatePreview();
				Factory.ReturnItem(gameState);
				return Factory.GetFactoryItem<SelectedItemGameState>(diContainer);
			}
			else
			{
				return gameState;
			}
		}
	}
}