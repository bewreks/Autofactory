using UnityEngine;

namespace Buildings.Models
{
	[CreateAssetMenu(fileName = "BaseGeneratorBuildingModel", menuName = "Models/Buildings/BaseGeneratorBuildingModel", order = 1)]
	public class BaseGeneratorBuildingModel : BuildingModel
	{
		[SerializeField] private float power;

		public float Power => power;
	}
}