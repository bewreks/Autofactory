using System.Collections.Generic;
using Buildings.Interfaces;
using Electricity.Interfaces;
using Helpers;
using UnityEngine;

namespace Electricity.Controllers
{
	public class ElectricalBuildingController : BuildingController, IElectricalBuildingController
	{
		private List<IElectricityNet> _nets;

		public IReadOnlyList<IElectricityNet>  Nets        => _nets;
		public List<IElectricalPoleController> NearlyPoles { get; }
		public IElectricalBuildingModel        Model       { get; }

		public ElectricalBuildingController(Vector3 position, IElectricalBuildingModel model) :
			base(position, model)
		{
			NearlyPoles = new List<IElectricalPoleController>();
			Model       = model;
			_nets       = new List<IElectricityNet>();
		}

		public void AddNet(IElectricityNet net)
		{
			_nets.AddUnique(net);
		}

		public void RemoveNet(IElectricityNet net)
		{
			_nets.Remove(net);
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