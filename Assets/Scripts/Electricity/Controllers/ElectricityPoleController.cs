using System.Collections.Generic;
using System.Linq;
using Buildings.Models;
using Helpers;
using UnityEngine;

namespace Electricity.Controllers
{
	public class ElectricityPoleController : BuildingController
	{
		public float                              Wires            { get; }
		public Rect                               Electricity      { get; }
		public ElectricPoleBuildingModel          Model            { get; }
		public List<ElectricityPoleController>    NearlyPoles      { get; }
		public List<GeneratorController>          NearlyGenerators { get; }
		public List<ElectricalBuildingController> NearlyBuildings  { get; }
		public ElectricityNet                     Net              { get; private set; }

		public ElectricityPoleController(Vector3 position, ElectricPoleBuildingModel model) : base(position, model)
		{
			NearlyPoles      = new List<ElectricityPoleController>();
			NearlyGenerators = new List<GeneratorController>();
			NearlyBuildings  = new List<ElectricalBuildingController>();
			Model            = model;
			Wires            = model.WireRadius;
			Electricity      = BuildingHelper.GetPoleRect(position, model.ElectricitySize);
		}

		public void AddGenerator(GeneratorController generator)
		{
			NearlyGenerators.Add(generator);
		}

		public void AddPole(ElectricityPoleController pole)
		{
			NearlyPoles.Add(pole);
		}

		public void RemoveGenerator(GeneratorController generator)
		{
			NearlyGenerators.Remove(generator);
		}

		public void RemovePole(ElectricityPoleController pole)
		{
			NearlyPoles.Remove(pole);
		}

		public bool InPoleWire(ElectricityPoleController pole)
		{
			var distance = Vector3.Distance(Position, pole.Position);
			return distance <= Wires ||
			       distance <= pole.Wires;
		}

		public void SetNet(ElectricityNet net)
		{
			Net = net;
		}

		public void AddPoles(ElectricityPoleController[] nearlyPoles)
		{
			foreach (var nearlyPole in nearlyPoles)
			{
				nearlyPole.AddPole(this);
			}

			NearlyPoles.AddUniqueRange(nearlyPoles);
		}

		public void AddGenerators(List<GeneratorController> generators)
		{
			NearlyGenerators.AddUniqueRange(generators);

			foreach (var generator in generators)
			{
				generator.AddPole(this);
			}
		}

		public void RemoveSelfFromNearlyPoles()
		{
			foreach (var nearlyPole in NearlyPoles)
			{
				nearlyPole.RemovePole(this);
			}
		}

		public void RemoveSelfFromNearlyGenerators()
		{
			foreach (var nearlyGenerator in NearlyGenerators)
			{
				nearlyGenerator.RemovePole(this);
				if (nearlyGenerator.NearlyPoles.Count(nearlyPole => nearlyPole.Net.ID == Net.ID) == 0)
				{
					nearlyGenerator.RemoveNet(Net);
				}
			}
		}

		public void AddBuildings(List<ElectricalBuildingController> buildings)
		{
			NearlyBuildings.AddUniqueRange(buildings);
			
			foreach (var building in buildings)
			{
				building.AddPole(this);
			}
		}
	}
}