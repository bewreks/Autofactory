using Buildings.Views;
using Inventories;
using UnityEngine;

namespace Buildings.Interfaces
{
	public interface IBuildingModel
	{
		public InventoryObjectsTypesEnum Type         { get; }
		public BuildingView              Instance     { get; }
		public Vector2                   BuildingSize { get; }
		string                           name         { get; }
	}
}