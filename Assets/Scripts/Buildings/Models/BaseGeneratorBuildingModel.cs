using Buildings.Interfaces;
using UnityEngine;

namespace Buildings.Models
{
	[CreateAssetMenu(fileName = "BaseGeneratorBuildingModel", menuName = "Models/Buildings/BaseGeneratorBuildingModel", order = 1)]
	public class BaseGeneratorBuildingModel : BuildingModel, IGeneratorModel
	{
		[SerializeField] private float power;

		public float Power => power;
	}
}