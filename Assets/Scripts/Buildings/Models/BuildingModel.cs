using System;
using Buildings.Views;
using Inventories;
using UnityEngine;

namespace Buildings.Models
{
	[CreateAssetMenu(fileName = "NewBuildingModel", menuName = "Models/Buildings/BaseBuildingModel", order = 0)]
	public class BuildingModel : ScriptableObject
	{
		[SerializeField] private InventoryObjectsTypesEnum type;
		[SerializeField] private BuildingView              instance;


		public InventoryObjectsTypesEnum Type         => type;
		public BuildingView              Instance     => instance;
		public Vector2                   BuildingSize { get; private set; }

		private void Awake()
		{
			if (instance.Collider)
			{
				var boxCollider = instance.Collider;
				var boxSize     = boxCollider.size;
				var size        = new Vector2(boxSize.x, boxSize.z) / 0.5f;
				
				size.x       = Mathf.Ceil(size.x);
				size.y       = Mathf.Ceil(size.y);
				BuildingSize = size;
			}
			else
			{
				BuildingSize = Vector2.one;
			}
		}
	}
}