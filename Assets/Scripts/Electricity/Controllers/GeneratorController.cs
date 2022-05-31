﻿using System;
using System.Collections.Generic;
using Buildings.Interfaces;
using Electricity.Interfaces;
using Helpers;
using ModestTree;
using UniRx;
using UnityEngine;

namespace Electricity.Controllers
{
	public class GeneratorController : BuildingController, IGeneratorController
	{
		private FloatReactiveProperty _actualPower = new FloatReactiveProperty();

		public IReadOnlyReactiveProperty<float> ActualPower => _actualPower;
		public IGeneratorModel                  Model       { get; }
		public List<ElectricityNet>             Nets        { get; }
		public List<IElectricalPoleController>  NearlyPoles { get; }

		public GeneratorController(Vector3 position, IGeneratorModel model) : base(position, model)
		{
			NearlyPoles       = new List<IElectricalPoleController>();
			Model             = model;
			Nets              = new List<ElectricityNet>();
			_actualPower.Value = model.Power;
		}

		public void AddNet(ElectricityNet net)
		{
			Nets.AddUnique(net);
			UpdatePowerPart();
		}

		public void RemoveNet(ElectricityNet net)
		{
			Nets.Remove(net);
			UpdatePowerPart();
		}

		private void UpdatePowerPart()
		{
			var old = ActualPower.Value;

			float newPart;
			if (Nets.IsEmpty())
			{
				newPart = Model.Power;
			}
			else
			{
				newPart = Model.Power / Nets.Count;
			}

			if (Math.Abs(old - newPart) > float.Epsilon)
			{
				_actualPower.SetValueAndForceNotify(newPart);
			}
		}

		public void AddPole(IElectricalPoleController pole)
		{
			NearlyPoles.AddUnique(pole);
		}

		public void RemovePole(IElectricalPoleController pole)
		{
			NearlyPoles.Remove(pole);
		}

		public void RemovePoles(List<IElectricalPoleController> poles)
		{
			NearlyPoles.RemoveAll(poles.Contains);
		}
	}
}