using System;
using System.Collections.Generic;
using Electricity.Interfaces;
using Helpers;

namespace Electricity
{
	public class ElectricityNet : IDisposable
	{
		public int   ID    { get; private set; }
		public float Power { get; private set; }

		private List<IElectricalPoleController>     _poles      = new List<IElectricalPoleController>();
		private List<IGeneratorController>          _generators = new List<IGeneratorController>();
		private List<IElectricalBuildingController> _buildings  = new List<IElectricalBuildingController>();

		public void Initialize(int netId)
		{
			ID = netId;
		}

		public void Initialize(int netId, List<IElectricalPoleController> poles)
		{
			Initialize(netId);
			_poles.AddRange(poles);
		}

		public void AddPole(IElectricalPoleController pole)
		{
			pole.SetNet(this);
			_poles.Add(pole);
		}

		public void RemovePole(IElectricalPoleController pole)
		{
			pole.SetNet(null);
			_poles.Remove(pole);
			foreach (var nearlyPole in pole.NearlyPoles)
			{
				nearlyPole.RemovePole(pole);
			}
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

		public void AddGenerators(List<IGeneratorController> generators)
		{
			_generators.AddUniqueRange(generators);
			generators.ForEach(generator => generator.AddNet(this));
		}

		public void AddBuildings(List<IElectricalBuildingController> buildings)
		{
			_buildings.AddUniqueRange(buildings);
			buildings.ForEach(building => building.AddNet(this));
		}

#if UNITY_INCLUDE_TESTS
		public List<IGeneratorController>          Generators => _generators;
		public List<IElectricalPoleController>     Poles      => _poles;
		public List<IElectricalBuildingController> Buildings  => _buildings;
#endif
	}
}