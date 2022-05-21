using System;
using Buildings.Models;

namespace Buildings.Views
{
	public class ElectricityConsumptionBuildingView : BuildingView
	{
		protected override Type ModelType => typeof(BuildingModel);
	}
}