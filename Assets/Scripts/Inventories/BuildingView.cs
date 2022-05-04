using UnityEngine;

namespace Inventories
{
	public class BuildingView : MonoBehaviour
	{
		[SerializeField] private Collider collider;
		[SerializeField] private Renderer renderer;

		public Collider Collider  => collider;
		public Renderer Renderer  => renderer;
		public bool     Triggered { get; private set; }

		private Color _color;
		private Color _error;

		private void Start()
		{
			var rendererMaterial = renderer.material;
			_color = rendererMaterial.color;
			_error = new Color(1, 0, 0, 0.5f);
		}

		private void OnTriggerEnter(Collider other)
		{
			Triggered = true;
			var rendererMaterial = renderer.material;
			rendererMaterial.color = _error;
		}

		private void OnTriggerExit(Collider other)
		{
			Triggered = false;
			var rendererMaterial = renderer.material;
			rendererMaterial.color = _color;
		}
	}
}