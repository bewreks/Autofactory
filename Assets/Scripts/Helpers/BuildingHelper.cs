using System;
using System.Collections.Generic;
using System.Linq;
using Electricity.Controllers;
using JetBrains.Annotations;
using UnityEngine;

namespace Helpers
{
	public static class BuildingHelper
	{
#if UNITY_INCLUDE_TESTS
		public static readonly string POLE_PATH              = "Models/Buildings/ElectricPoleBuildingModel";
		public static readonly string GENERATOR_PATH         = "Models/Buildings/BaseGeneratorBuildingModel";
		public static readonly string ELECTRIC_BUILDING_PATH = "Models/Buildings/ElectricBuildingModel";
#endif


		public static readonly float UnitSize     = 1f;
		public static readonly float UnitHalfSize = UnitSize / 2;

		public static Vector2 GetBuildingSize(this Vector3 size)
		{
			var newSize = new Vector2(size.x, size.z);

			newSize.x = Mathf.Ceil(newSize.x);
			newSize.y = Mathf.Ceil(newSize.y);
			return newSize;
		}

		public static Rect GetGeneratorRect(Vector3 position, Vector2 buildingSize)
		{
			return new Rect(new Vector2(position.x, position.z), buildingSize);
		}

		public static Rect GetPoleRect(Vector3 position, float size)
		{
			var halfSize = size / 2;
			return new Rect(new Vector2(position.x - halfSize + UnitHalfSize, position.z - halfSize + UnitHalfSize),
			                new Vector2(size,                                 size));
		}

		public static void DrawGizmo(this Rect rect, Color color)
		{
#if UNITY_EDITOR
			Gizmos.color = color;

			Gizmos.DrawLine(new Vector3(rect.xMin, 0, rect.yMin),
			                new Vector3(rect.xMin, 0, rect.yMax));
			Gizmos.DrawLine(new Vector3(rect.xMin, 0, rect.yMax),
			                new Vector3(rect.xMax, 0, rect.yMax));
			Gizmos.DrawLine(new Vector3(rect.xMax, 0, rect.yMax),
			                new Vector3(rect.xMax, 0, rect.yMin));
			Gizmos.DrawLine(new Vector3(rect.xMax, 0, rect.yMin),
			                new Vector3(rect.xMin, 0, rect.yMin));
			Gizmos.DrawLine(new Vector3(rect.xMin, 0, rect.yMin),
			                new Vector3(rect.xMax, 0, rect.yMax));
			Gizmos.DrawLine(new Vector3(rect.xMin, 0, rect.yMax),
			                new Vector3(rect.xMax, 0, rect.yMin));
#endif
		}
	}
}