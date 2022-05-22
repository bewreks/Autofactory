using UnityEngine;

namespace Buildings.Colliders
{
	public class ElectricitySquareCollider : MonoBehaviour
	{
		private void OnTriggerEnter(Collider other)
		{
			Debug.Log($"Square collide with {other.name}");
		}

		private void OnTriggerExit(Collider other)
		{
			Debug.Log($"Square stop colliding with {other.name}");
		}
	}
}