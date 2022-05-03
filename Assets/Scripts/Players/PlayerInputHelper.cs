using UnityEngine;

namespace Players
{
	public static class PlayerInputHelper
	{
		public static bool GetWorldMousePosition(LayerMask groundMask, Camera castCamera, out Vector3 mousePosition)
		{
			mousePosition = Vector3.zero;

			var ray = castCamera.ScreenPointToRay(Input.mousePosition);

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

		public static Vector3 GetPlayerInput(Camera camera, float delta)
		{
			var transform = camera.transform;
			var forward   = transform.forward;
			var right     = transform.right;

			forward.y = 0;
			right.y   = 0;
			forward.Normalize();
			right.Normalize();

			right   *= Input.GetAxis("Horizontal");
			forward *= Input.GetAxis("Vertical");

			forward += right;
			
			forward.Normalize();

			return forward * delta;
		}
	}
}