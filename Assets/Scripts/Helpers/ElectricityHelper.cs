using System.Collections.Generic;
using System.Linq;
using Electricity.Controllers;

namespace Helpers
{
	public static class ElectricityHelper
	{
		public static void UnitePoles(this List<ElectricityPoleController> poles, List<ElectricityPoleController> newNet)
		{
			foreach (var pole in poles.Where(pole => !newNet.Contains(pole)))
			{
				newNet.Add(pole);
				pole.NearlyPoles.UnitePoles(newNet);
			}
		}
	}
}