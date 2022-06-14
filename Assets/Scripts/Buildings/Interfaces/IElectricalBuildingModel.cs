using UnityEngine;

namespace Buildings.Interfaces
{
	public interface IElectricalBuildingModel : IBuildingModel
	{
		public AnimationCurve ConsumptionCurve { get; }
	}
}