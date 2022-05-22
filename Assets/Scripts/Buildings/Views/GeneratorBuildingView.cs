﻿using System;
using Buildings.Models;
using Electricity;
using UnityEngine;
using Zenject;

namespace Buildings.Views
{
	public class GeneratorBuildingView : BuildingView
	{
		[Inject] private ElectricityController _electricityController;

		protected override Type ModelType     => typeof(BaseGeneratorBuildingModel);
		protected override int  BuildingLayer => gameSettings.GeneratorLayer;

		protected override void OnFinalInstantiate()
		{
			var transformCache = transform;
			var generatorModel = (BaseGeneratorBuildingModel)_model;

			_electricityController.AddGenerator(transformCache.position, generatorModel);
		}

		private void OnDrawGizmos()
		{
			var rect = BuildingHelper.GetGeneratorRect(transform.position, Collider.size.GetBuildingSize());
			rect.DrawGizmo(Color.red);
		}
	}
}