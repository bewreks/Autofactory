using System;
using System.Collections.Generic;
using Buildings.Models;
using ModestTree;
using UnityEngine;

namespace Electricity.Controllers
{
	public class GeneratorController
	{
		public event Action<float, float> ChangeOfPower;
		public Vector3                    Position      { get; }
		public BaseGeneratorBuildingModel Model         { get; }
		public float                      PartOfPower   { get; private set; }
		public List<ElectricityNet>       Nets          { get; }
		public Rect                       BuildingRect { get; }

		public GeneratorController(Vector3 position, BaseGeneratorBuildingModel model)
		{
			Position      = position;
			Model         = model;
			PartOfPower   = model.Power;
			Nets          = new List<ElectricityNet>();
			BuildingRect = BuildingHelper.GetGeneratorRect(position, model.BuildingSize);
		}

		public void AddNet(ElectricityNet net)
		{
			if (!Nets.Contains(net))
			{
				Nets.Add(net);
				UpdatePowerPart();
			}
		}

		public void RemoveNet(ElectricityNet net)
		{
			if (!Nets.Contains(net))
			{
				Nets.Remove(net);
				UpdatePowerPart();
			}
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
	}
}