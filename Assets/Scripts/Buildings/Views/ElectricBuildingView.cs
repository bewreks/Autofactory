using System;
using Buildings.Interfaces;
using Buildings.Models;
using Electricity;
using Electricity.Controllers;
using Zenject;

namespace Buildings.Views
{
	public class ElectricBuildingView : BuildingView
	{
		[Inject] private IElectricityController _electricityController;

		protected override Type ModelType     => typeof(ElectricalBuildingModel);
		protected override int  BuildingLayer => gameSettings.ConsumptionLayer;

		public ElectricalBuildingController        BuildingController { get; private set; }
		public IElectricalBuildingModel BuildingModel      => (IElectricalBuildingModel)Model;

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