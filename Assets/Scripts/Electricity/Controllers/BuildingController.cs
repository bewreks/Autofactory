using Buildings.Models;
using Helpers;
using UnityEngine;

namespace Electricity.Controllers
{
	public abstract class BuildingController
	{
		public Vector3              Position     { get; }
		public Rect                 BuildingRect { get; }

		protected BuildingController(Vector3 position, BuildingModel model)
		{
			Position     = position;
			BuildingRect = BuildingHelper.GetGeneratorRect(position, model.BuildingSize);
		}
	}
}