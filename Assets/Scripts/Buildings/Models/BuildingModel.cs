using Buildings.Views;
using Helpers;
using Inventories;
using UnityEngine;

namespace Buildings.Models
{
	[CreateAssetMenu(fileName = "NewBuildingModel", menuName = "Models/Buildings/BaseBuildingModel", order = 0)]
	public class BuildingModel : ScriptableObject
	{
		[SerializeField] private InventoryObjectsTypesEnum type;
		[SerializeField] private BuildingView          instance;


		public InventoryObjectsTypesEnum   Type         => type;
		public BuildingView Instance     => instance;
		public Vector2                     BuildingSize { get; private set; }

		public virtual void Awake()
		{
			if (instance.Collider)
			{
				BuildingSize = instance.Collider.size.GetBuildingSize();
			}
			else
			{
				BuildingSize = Vector2.one;
			}
		}
	}
}