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
		public float                                        Wires            { get; }
		public Rect                                         Electricity      { get; }
		public IElectricalPoleModel                         Model            { get; }
		public IReadOnlyList<IElectricalPoleController>     NearlyPoles      => _poles;
		public IReadOnlyList<IGeneratorController>          NearlyGenerators => _generators;
		public IReadOnlyList<IElectricalBuildingController> NearlyBuildings  => _buildings;
		public IElectricityNet                              Net              { get; private set; }

		private List<IElectricalPoleController>     _poles;
		private List<IGeneratorController>          _generators;
		private List<IElectricalBuildingController> _buildings;

		public ElectricalPoleController(Vector3 position, IElectricalPoleModel model) : base(position, model)
		{
			_poles      = new List<IElectricalPoleController>();
			_generators = new List<IGeneratorController>();
			_buildings  = new List<IElectricalBuildingController>();
			Model       = model;
			Wires       = model.WireRadius;
			Electricity = BuildingHelper.GetPoleRect(position, model.ElectricitySize);
		}

		public void AddGenerator(IGeneratorController generator)
		{
			_generators.AddUnique(generator);
		}

		public void RemoveGenerator(IGeneratorController generator)
		{
			_generators.Remove(generator);
		}

		public void AddGenerators(List<IGeneratorController> generators)
		{
			_generators.AddUniqueRange(generators);
		}

		public void RemoveGenerators(List<IGeneratorController> generators)
		{
			_generators.RemoveAll(generators.Contains);
		}

		public void AddPole(IElectricalPoleController pole)
		{
			_poles.AddUnique(pole);
		}

		public void RemovePole(IElectricalPoleController pole)
		{
			_poles.Remove(pole);
		}

		public void AddPoles(IElectricalPoleController[] nearlyPoles)
		{
			_poles.AddUniqueRange(nearlyPoles);
		}

		public void RemovePoles(IEnumerable<IElectricalPoleController> nearlyPoles)
		{
			_poles.RemoveAll(nearlyPoles.Contains);
		}

		public void SetNet(IElectricityNet net)
		{
			Net = net;
		}

		public void AddBuilding(IElectricalBuildingController building)
		{
			_buildings.AddUnique(building);
		}

		public void RemoveBuilding(IElectricalBuildingController building)
		{
			_buildings.Remove(building);
		}

		public void AddBuildings(List<IElectricalBuildingController> buildings)
		{
			_buildings.AddUniqueRange(buildings);
		}

		public void RemoveBuildings(List<IElectricalBuildingController> buildings)
		{
			_buildings.RemoveAll(buildings.Contains);
		}
	}
}