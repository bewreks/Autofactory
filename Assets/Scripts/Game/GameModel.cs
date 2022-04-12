using Inventory;
using UnityEngine;

namespace Game
{
	public class GameModel : IGameModel
	{
		public FullInventoryPack SelectedPack     { get; set; }
		public GameObject        InstantiablePack { get; set; }
	}

	public interface IGameModel
	{
		FullInventoryPack SelectedPack     { get; }
		GameObject        InstantiablePack { get; }
	}
}