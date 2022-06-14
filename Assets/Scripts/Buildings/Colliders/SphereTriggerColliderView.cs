using UnityEngine;

namespace Buildings.Colliders
{
	public class SphereTriggerColliderView : ColliderView
	{
		public override void SetSize(float size)
		{
			((SphereCollider)_triggerCollider).radius = size;
		}
	}
}