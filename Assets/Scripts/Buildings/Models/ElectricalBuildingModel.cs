using Buildings.Interfaces;
using UnityEngine;

namespace Buildings.Models
{
	[CreateAssetMenu(fileName = "ElectricityConsumptionBuildingModel", menuName = "Models/Buildings/ElectricityConsumptionBuildingModel", order = 2)]
	public class ElectricalBuildingModel : BuildingModel, IElectricalBuildingModel
	{
		[SerializeField] private AnimationCurve curve;

		public AnimationCurve ConsumptionCurve => curve;
	}
}