using System;
using System.Collections.Generic;
using Buildings.Models;
using ModestTree;
using UnityEngine;

namespace Electricity.Controllers
{
	public class GeneratorController : BuildingController
	{
		public event Action<float, float>      ChangeOfPower;
		public BaseGeneratorBuildingModel      Model       { get; }
		public float                           PartOfPower { get; private set; }
		public List<ElectricityNet>            Nets        { get; }
		public List<ElectricityPoleController> NearlyPoles { get; }

		public GeneratorController(Vector3 position, BaseGeneratorBuildingModel model) : base(position, model)
		{
			NearlyPoles = new List<ElectricityPoleController>();
			Model       = model;
			PartOfPower = model.Power;
			Nets        = new List<ElectricityNet>();
		}

		public void AddNet(ElectricityNet net)
		{
			Nets.Add(net);
			UpdatePowerPart();
		}

		public void RemoveNet(ElectricityNet net)
		{
			Nets.Remove(net);
			UpdatePowerPart();
		}

		private void UpdatePowerPart()
		{
			var oldPart = PartOfPower;
			if (Nets.IsEmpty())
			{
				PartOfPower = Model.Power;
			}
			else
			{
				PartOfPower = Model.Power / Nets.Count;
			}

			if (Math.Abs(oldPart - PartOfPower) > float.Epsilon)
			{
				ChangeOfPower?.Invoke(oldPart, PartOfPower);
			}
		}

		public void AddPole(ElectricityPoleController pole)
		{
			NearlyPoles.Add(pole);
		}

		public void RemovePole(ElectricityPoleController pole)
		{
			NearlyPoles.Remove(pole);
		}
	}
}