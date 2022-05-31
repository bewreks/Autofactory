using System;
using Buildings.Models;
using Electricity;
using Electricity.Controllers;
using Zenject;

namespace Buildings.Views
{
	public class ElectricBuildingView : BuildingView
	{
		[Inject] private IElectricityController _electricityController;

		protected override Type ModelType     => typeof(ElectricBuildingModel);
		protected override int  BuildingLayer => gameSettings.ConsumptionLayer;

		public ElectricalBuildingController        BuildingController { get; private set; }
		public ElectricBuildingModel BuildingModel      => (ElectricBuildingModel)Model;

		protected override void OnFinalInstantiate()
		{
			BuildingController = new ElectricalBuildingController(transform.position, BuildingModel);

			_electricityController.AddBuilding(BuildingController);
		}

		protected override void OnRemoveInstance()
		{
			_electricityController.RemoveBuilding(BuildingController);
		}
	}
}