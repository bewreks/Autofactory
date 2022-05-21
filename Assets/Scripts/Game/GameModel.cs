using System;
using Buildings;
using Buildings.Models;
using Buildings.Views;
using Inventories;
using Players;
using UnityEngine;

namespace Game
{
	public class GameModel : IGameModel
	{
		public FullInventoryPack           SelectedPack     { get; set; }
		public BuildingView InstantiablePack { get; set; }
		public PlayerModel                 PlayerModel      { get; set; }
		public Vector3                     MousePosition    { get; set; }
		public Vector3                     MoveDelta        { get; set; }

		public void Dispose()
		{
			SelectedPack?.Dispose();
			if (PlayerModel)
				PlayerModel.Dispose();
		}
	}

	public interface IGameModel : IDisposable
	{
		FullInventoryPack           SelectedPack     { get; }
		BuildingView InstantiablePack { get; }
		PlayerModel                 PlayerModel      { get; }
		Vector3                     MousePosition    { get; }
		Vector3                     MoveDelta        { get; }
	}
}