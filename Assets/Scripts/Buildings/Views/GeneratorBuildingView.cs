﻿using System;
using Buildings.Models;
using Electricity;
using Zenject;

namespace Buildings.Views
{
	public class GeneratorBuildingView : BuildingView
	{
		[Inject] private ElectricityController _electricityController;
		
		public override void SetModel(BuildingModel model)
		{
			base.SetModel(model);

			if (!(_model is BaseGeneratorBuildingModel))
			{
				throw new NullReferenceException($"{model.name} is not electric model");
			}
		}

		public override void FinalInstantiate()
		{
			var transformCache = transform;
			var generatorModel  = (BaseGeneratorBuildingModel)_model;

			_electricityController.AddGenerator(transformCache.position, generatorModel);
		}
	}
}