using System;
using System.Collections.Generic;
using Electricity.Controllers;
using Helpers;

namespace Electricity
{
	public class ElectricityNet : IDisposable
	{
		public int   ID    { get; private set; }
		public float Power { get; private set; }

		private List<ElectricityPoleController>    _poles      = new List<ElectricityPoleController>();
		private List<GeneratorController>          _generators = new List<GeneratorController>();
		private List<ElectricalBuildingController> _buildings  = new List<ElectricalBuildingController>();

		public void Initialize(int netId)
		{
			ID = netId;
		}

		public void Initialize(int netId, List<ElectricityPoleController> poles)
		{
			Initialize(netId);
			_poles.AddRange(poles);
		}

		public void AddPole(ElectricityPoleController pole)
		{
			pole.SetNet(this);
			_poles.Add(pole);
		}

		public void RemovePole(ElectricityPoleController pole)
		{
			pole.SetNet(null);
			_poles.Remove(pole);
			pole.RemoveSelfFromNearlyPoles();
			pole.NearlyPoles.Clear();
		}

		public void Unite(ElectricityNet from)
		{
			_poles.AddRange(from._poles);
			_buildings.AddRange(from._buildings);
			_generators.AddRange(from._generators);
		}

		public void Dispose()
		{
			_poles.Clear();
			ID = -1;
		}

		public void AddGenerators(List<GeneratorController> generators)
		{
			_generators.AddUniqueRange(generators);
			generators.ForEach(generator => generator.AddNet(this));
		}

		public void AddBuildings(List<ElectricalBuildingController> buildings)
		{
			_buildings.AddUniqueRange(buildings);
			buildings.ForEach(building => building.AddNet(this));
		}

#if UNITY_INCLUDE_TESTS
		public List<GeneratorController>          Generators => _generators;
		public List<ElectricityPoleController>    Poles      => _poles;
		public List<ElectricalBuildingController> Buildings  => _buildings;
#endif
	}
}