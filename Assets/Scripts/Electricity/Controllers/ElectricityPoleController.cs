using Buildings.Models;
using UnityEngine;

namespace Electricity.Controllers
{
	public class ElectricityPoleController
	{
		public float                     Wires       { get; }
		public Rect                      Electricity { get; }
		public Vector3                   Position    { get; }
		public ElectricPoleBuildingModel Model       { get; }

		public ElectricityPoleController(Vector3 position, ElectricPoleBuildingModel model)
		{
			Model       = model;
			Position    = position;
			Wires       = model.WireRadius;
			Electricity = BuildingHelper.GetPoleRect(position, model.ElectricitySize);
		}
	}
}