using Inventory;
using UnityEngine;

namespace Game
{
	public class GameModel : IGameModel
	{
		public InventoryPack SelectedPack     { get; set; }
		public GameObject    InstantiablePack { get; set; }
	}

	public interface IGameModel
	{
		InventoryPack SelectedPack     { get; }
		GameObject    InstantiablePack { get; }
	}
}