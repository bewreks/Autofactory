using System;
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
		private ReactiveProperty<(IGeneratorController, float)> _actualPower =
			new ReactiveProperty<(IGeneratorController, float)>();

		public IReadOnlyReactiveProperty<(IGeneratorController, float)> ActualPower => _actualPower;
		public IGeneratorModel                                          Model       { get; }
		public List<IElectricityNet>                                    Nets        { get; }
		public List<IElectricalPoleController>                          NearlyPoles { get; }

		public GeneratorController(Vector3 position, IGeneratorModel model) : base(position, model)
		{
			NearlyPoles        = new List<IElectricalPoleController>();
			Model              = model;
			Nets               = new List<IElectricityNet>();
			_actualPower.Value = (this, model.Power);
		}

		public void AddNet(IElectricityNet net)
		{
			Nets.AddUnique(net);
			UpdatePowerPart();
		}

		public void RemoveNet(IElectricityNet net)
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

			if (Math.Abs(old.Item2 - newPart) > float.Epsilon)
			{
				_actualPower.SetValueAndForceNotify((this, newPart));
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