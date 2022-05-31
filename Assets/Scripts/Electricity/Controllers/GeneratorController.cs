using System;
using System.Collections.Generic;
using System.Linq;
using Buildings.Models;
using Helpers;
using ModestTree;
using UniRx;
using UnityEngine;

namespace Electricity.Controllers
{
	public class GeneratorController : BuildingController
	{
		public FloatReactiveProperty ActualPower = new FloatReactiveProperty();

		public BaseGeneratorBuildingModel      Model       { get; }
		public List<ElectricityNet>            Nets        { get; }
		public List<ElectricityPoleController> NearlyPoles { get; }

		public GeneratorController(Vector3 position, BaseGeneratorBuildingModel model) : base(position, model)
		{
			NearlyPoles       = new List<ElectricityPoleController>();
			Model             = model;
			Nets              = new List<ElectricityNet>();
			ActualPower.Value = model.Power;
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
				ActualPower.SetValueAndForceNotify(newPart);
			}
		}

		public void AddPole(ElectricityPoleController pole)
		{
			NearlyPoles.AddUnique(pole);
		}

		public void RemovePole(ElectricityPoleController pole)
		{
			NearlyPoles.Remove(pole);
		}

		public void RemovePoles(List<ElectricityPoleController> poles)
		{
			NearlyPoles.RemoveAll(poles.Contains);
		}
	}
}