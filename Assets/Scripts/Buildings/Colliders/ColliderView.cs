using UnityEngine;

namespace Buildings.Colliders
{
	public abstract class ColliderView : MonoBehaviour
	{
		[SerializeField] protected Collider _triggerCollider;

		public Collider TriggerCollider => _triggerCollider;

		public abstract void SetSize(float size);
	}
}