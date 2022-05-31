using System.Collections.Generic;
using Buildings.Models;
using UnityEngine;

namespace Electricity.Controllers
{
	public class ElectricalBuildingController : BuildingController
	{
		public List<ElectricityNet>            Nets        { get; }
		public List<ElectricityPoleController> NearlyPoles { get; }
		public ElectricBuildingModel           Model       { get; }

		public ElectricalBuildingController(Vector3 position, ElectricBuildingModel model) :
			base(position, model)
		{
			NearlyPoles = new List<ElectricityPoleController>();
			Model       = model;
			Nets        = new List<ElectricityNet>();
		}

		public void AddNet(ElectricityNet electricityNet)
		{
			throw new System.NotImplementedException();
		}

		public void AddPole(ElectricityPoleController electricityPoleController)
		{
			throw new System.NotImplementedException();
		}

		public void RemovePoles(List<ElectricityPoleController> poles)
		{
			throw new System.NotImplementedException();
		}
	}
}