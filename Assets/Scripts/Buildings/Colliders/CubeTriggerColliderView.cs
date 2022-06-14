using UnityEngine;

namespace Buildings.Colliders
{
	public class CubeTriggerColliderView : ColliderView
	{
		public override void SetSize(float size)
		{
			((BoxCollider)_triggerCollider).size = Vector3.one * size;
		}
	}
}