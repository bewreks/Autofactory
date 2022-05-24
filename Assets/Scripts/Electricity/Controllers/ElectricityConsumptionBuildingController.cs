using Buildings.Models;
using UnityEngine;

namespace Electricity.Controllers
{
	public class ElectricityConsumptionBuildingController : BuildingController
	{
		public ElectricityConsumptionBuildingController(Vector3 position, ElectricityConsumptionBuildingModel model) : base(position, model) { }
	}
}