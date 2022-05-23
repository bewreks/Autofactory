using Buildings.Views;
using Electricity;
using UnityEngine;
using Zenject;

namespace Buildings.Colliders
{
	public class ElectricitySquareCollider : MonoBehaviour
	{
		[Inject] private ElectricityController_old _electricityController;

		public ElectricPoleBuildingView Pole { get; set; }
		
		private void OnTriggerEnter(Collider other)
		{
			var generatorBuildingView = other.GetComponent<GeneratorBuildingView>();
			if (generatorBuildingView)
			{
				_electricityController.AddGenerator(generatorBuildingView.GeneratorController, Pole.NetID);
			}

			var consumptionBuildingView = other.GetComponent<ElectricityConsumptionBuildingView>();
			if (consumptionBuildingView)
			{
				// закинуть это здание в элекросеть
			}

			Debug.Log($"Square collide with {other.name}");
		}

		private void OnTriggerExit(Collider other)
		{
			var generatorBuildingView = other.GetComponent<GeneratorBuildingView>();
			if (generatorBuildingView)
			{
				_electricityController.RemoveGenerator(generatorBuildingView.GeneratorController);
			}

			var consumptionBuildingView = other.GetComponent<ElectricityConsumptionBuildingView>();
			if (consumptionBuildingView)
			{
				// убрать это здание из элекросети
			}
			
			Debug.Log($"Square stop colliding with {other.name}");
		}
	}
}