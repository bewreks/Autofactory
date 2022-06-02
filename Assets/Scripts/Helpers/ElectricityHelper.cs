using System.Collections.Generic;
using System.Linq;
using Electricity.Interfaces;

namespace Helpers
{
	public static class ElectricityHelper
	{
		public static void UnitePoles(this List<IElectricalPoleController> poles,
		                              List<IElectricalPoleController>      newNet)
		{
			foreach (var pole in poles.Where(pole => !newNet.Contains(pole)))
			{
				newNet.Add(pole);
				pole.NearlyPoles.UnitePoles(newNet);
			}
		}

		public static List<IGeneratorController> AllGenerators(this List<IElectricalPoleController> poles)
		{
			var generators = new List<IGeneratorController>();
			foreach (var pole in poles)
			{
				generators.AddRange(pole.NearlyGenerators);
			}

			return generators.Distinct().ToList();
		}

		public static List<IElectricalBuildingController> AllBuildings(this List<IElectricalPoleController> poles)
		{
			var buildings = new List<IElectricalBuildingController>();
			foreach (var pole in poles)
			{
				buildings.AddRange(pole.NearlyBuildings);
			}

			return buildings.Distinct().ToList();
		}
	}
}