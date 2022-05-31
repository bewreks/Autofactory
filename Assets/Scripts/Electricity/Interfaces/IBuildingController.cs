using UnityEngine;

namespace Electricity.Interfaces
{
	public interface IBuildingController
	{
		public Vector3 Position     { get; }
		public Rect    BuildingRect { get; }
	}
}