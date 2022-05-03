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

		public static Vector3 GetPlayerInput(float delta)
		{
			return new Vector3(Input.GetAxis("Horizontal") * delta,
			                   0,
			                   Input.GetAxis("Vertical") * delta);
		}
	}
}