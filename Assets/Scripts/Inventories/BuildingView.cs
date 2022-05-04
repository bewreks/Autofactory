using UnityEngine;

namespace Inventories
{
	public class BuildingView : MonoBehaviour
	{
		[SerializeField] private Collider collider;
		[SerializeField] private Renderer renderer;

		public Collider Collider => collider;
		public Renderer Renderer => renderer;
	}
}