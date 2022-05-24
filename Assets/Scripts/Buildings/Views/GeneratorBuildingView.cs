using System;
using Buildings.Models;
using Electricity;
using Electricity.Controllers;
using UnityEngine;
using Zenject;

namespace Buildings.Views
{
	public class GeneratorBuildingView : BuildingView
	{
		[Inject] private IElectricityController _electricityController;

		protected override Type ModelType     => typeof(BaseGeneratorBuildingModel);
		protected override int  BuildingLayer => gameSettings.GeneratorLayer;

		public BaseGeneratorBuildingModel GeneratorModel      => (BaseGeneratorBuildingModel)_model;
		public GeneratorController        GeneratorController { get; private set; }
		
		protected override void OnFinalInstantiate()
		{
			GeneratorController = new GeneratorController(transform.position, GeneratorModel);
			
			_electricityController.AddGenerator(GeneratorController);
		}

		private void OnDrawGizmos()
		{
			var rect = BuildingHelper.GetGeneratorRect(transform.position, Collider.size.GetBuildingSize());
			rect.DrawGizmo(Color.red);
		}
	}
}