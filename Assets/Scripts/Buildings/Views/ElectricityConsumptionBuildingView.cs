using System;
using Buildings.Models;
using Electricity.Controllers;

namespace Buildings.Views
{
	public class ElectricityConsumptionBuildingView : BuildingView
	{
		protected override Type                                     ModelType => typeof(ElectricityConsumptionBuildingModel);
		protected override int                                      BuildingLayer => gameSettings.ConsumptionLayer;
		
		public ElectricityConsumptionBuildingController BuildingController { get; private set; }
		public ElectricityConsumptionBuildingModel      BuildingModel => (ElectricityConsumptionBuildingModel)Model;

		protected override void OnFinalInstantiate()
		{
			BuildingController = new ElectricityConsumptionBuildingController(transform.position, BuildingModel);
		}
	}
}