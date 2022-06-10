using UnityEngine;

namespace Buildings.Colliders
{
	public class ColliderView : MonoBehaviour
	{
		[SerializeField] private Collider _triggerCollider;

		public Collider TriggerCollider => _triggerCollider;
	}
}