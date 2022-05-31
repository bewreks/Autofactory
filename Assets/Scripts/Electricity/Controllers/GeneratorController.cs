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
		public  Subject<float> ActualPower = new Subject<float>();
		private float          _oldValue;
		
		public BaseGeneratorBuildingModel      Model       { get; }
		public List<ElectricityNet>            Nets        { get; }
		public List<ElectricityPoleController> NearlyPoles { get; }

		public GeneratorController(Vector3 position, BaseGeneratorBuildingModel model) : base(position, model)
		{
			NearlyPoles = new List<ElectricityPoleController>();
			Model       = model;
			Nets        = new List<ElectricityNet>();
			ActualPower.OnNext(model.Power);
			_oldValue = 0;
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
			float newPart;
			if (Nets.IsEmpty())
			{
				newPart = Model.Power;
			}
			else
			{
				newPart = Model.Power / Nets.Count;
			}

			if (Math.Abs(_oldValue - newPart) > float.Epsilon)
			{
				_oldValue = newPart;
				ActualPower.OnNext(newPart);
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
			foreach (var pole in poles.Where(pole => !NearlyPoles.Select(_ => _.Net).Distinct().Contains(pole.Net)))
			{
				Nets.Remove(pole.Net);
			}
		}
	}
}