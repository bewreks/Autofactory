using System.Collections.Generic;
using Buildings.Interfaces;
using Electricity.Interfaces;
using Helpers;
using UnityEngine;

namespace Electricity.Controllers
{
	public class ElectricalBuildingController : BuildingController, IElectricalBuildingController
	{
		public List<ElectricityNet>            Nets        { get; }
		public List<IElectricalPoleController> NearlyPoles { get; }
		public IElectricalBuildingModel        Model       { get; }

		public ElectricalBuildingController(Vector3 position, IElectricalBuildingModel model) :
			base(position, model)
		{
			NearlyPoles = new List<IElectricalPoleController>();
			Model       = model;
			Nets        = new List<ElectricityNet>();
		}

		public void AddNet(ElectricityNet net)
		{
			Nets.AddUnique(net);
		}

		public void RemoveNet(ElectricityNet net)
		{
			Nets.Remove(net);
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