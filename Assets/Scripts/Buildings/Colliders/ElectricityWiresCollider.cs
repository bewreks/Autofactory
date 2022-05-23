using Buildings.Views;
using Electricity;
using UnityEngine;
using Zenject;

namespace Buildings.Colliders
{
	public class ElectricityWiresCollider : MonoBehaviour
	{
		[Inject] private ElectricityController_old _electricityController;

		public ElectricPoleBuildingView Pole { get; set; }
		
		private void OnTriggerEnter(Collider other)
		{
			Debug.Log($"Square collide with {other.name}");
		}

		private void OnTriggerExit(Collider other)
		{
			Debug.Log($"Square collide with {other.name}");
		}
	}
}