using Buildings.Interfaces;
using Electricity.Interfaces;
using Helpers;
using UnityEngine;

namespace Electricity.Controllers
{
	public abstract class BuildingController : IBuildingController
	{
		public Vector3 Position     { get; }
		public Rect    BuildingRect { get; }

		protected BuildingController(Vector3 position, IBuildingModel model)
		{
			Position     = position;
			BuildingRect = BuildingHelper.GetGeneratorRect(position, model.BuildingSize);
		}
	}
}