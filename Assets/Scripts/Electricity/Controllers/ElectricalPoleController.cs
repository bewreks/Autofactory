using System.Collections.Generic;
using System.Linq;
using Buildings.Interfaces;
using Electricity.Interfaces;
using Helpers;
using UnityEngine;

namespace Electricity.Controllers
{
	public class ElectricalPoleController : BuildingController, IElectricalPoleController
	{
		public float                               Wires            { get; }
		public Rect                                Electricity      { get; }
		public IElectricalPoleModel                Model            { get; }
		public List<IElectricalPoleController>     NearlyPoles      { get; }
		public List<IGeneratorController>          NearlyGenerators { get; }
		public List<IElectricalBuildingController> NearlyBuildings  { get; }
		public ElectricityNet                      Net              { get; private set; }


		public ElectricalPoleController(Vector3 position, IElectricalPoleModel model) : base(position, model)
		{
			NearlyPoles      = new List<IElectricalPoleController>();
			NearlyGenerators = new List<IGeneratorController>();
			NearlyBuildings  = new List<IElectricalBuildingController>();
			Model            = model;
			Wires            = model.WireRadius;
			Electricity      = BuildingHelper.GetPoleRect(position, model.ElectricitySize);
		}

		public void AddGenerator(IGeneratorController generator)
		{
			NearlyGenerators.AddUnique(generator);
		}

		public void RemoveGenerator(IGeneratorController generator)
		{
			NearlyGenerators.Remove(generator);
		}

		public void AddGenerators(List<IGeneratorController> generators)
		{
			NearlyGenerators.AddUniqueRange(generators);
		}

		public void RemoveGenerators(List<IGeneratorController> generators)
		{
			NearlyGenerators.RemoveAll(generators.Contains);
		}

		public void AddPole(IElectricalPoleController pole)
		{
			NearlyPoles.AddUnique(pole);
		}

		public void RemovePole(IElectricalPoleController pole)
		{
			NearlyPoles.Remove(pole);
		}

		public void AddPoles(IElectricalPoleController[] nearlyPoles)
		{
			NearlyPoles.AddUniqueRange(nearlyPoles);
		}

		public void RemovePoles(IElectricalPoleController[] nearlyPoles)
		{
			NearlyPoles.RemoveAll(nearlyPoles.Contains);
		}

		public void SetNet(ElectricityNet net)
		{
			Net = net;
		}

		public void AddBuilding(IElectricalBuildingController building)
		{
			NearlyBuildings.AddUnique(building);
		}

		public void RemoveBuilding(IElectricalBuildingController building)
		{
			NearlyBuildings.Remove(building);
		}

		public void AddBuildings(List<IElectricalBuildingController> buildings)
		{
			NearlyBuildings.AddUniqueRange(buildings);
		}

		public void RemoveBuildings(List<IElectricalBuildingController> buildings)
		{
			NearlyBuildings.RemoveAll(buildings.Contains);
		}
	}
}