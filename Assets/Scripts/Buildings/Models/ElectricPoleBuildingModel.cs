using UnityEngine;

namespace Buildings.Models
{
	[CreateAssetMenu(fileName = "ElectricPoleBuildingModel", menuName = "Models/Buildings/ElectricPoleBuildingModel", order = 0)]
	public class ElectricPoleBuildingModel : BuildingModel
	{
		[SerializeField] private float wireRadius;
		[SerializeField] private float electricitySize;

		public float WireRadius      => wireRadius;
		public float ElectricitySize => electricitySize;
	}
}