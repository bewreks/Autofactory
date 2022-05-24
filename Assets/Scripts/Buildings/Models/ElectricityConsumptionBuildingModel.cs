﻿using UnityEngine;

namespace Buildings.Models
{
	[CreateAssetMenu(fileName = "ElectricityConsumptionBuildingModel", menuName = "Models/Buildings/ElectricityConsumptionBuildingModel", order = 2)]
	public class ElectricityConsumptionBuildingModel : BuildingModel
	{
		[SerializeField] private AnimationCurve curve;

		public AnimationCurve Curve => curve;
	}
}