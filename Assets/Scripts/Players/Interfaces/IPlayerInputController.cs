using System;
using UnityEngine;

namespace Players.Interfaces
{
	public interface IPlayerInputController : IDisposable
	{
		public Vector2 MousePosition     { get; }
		public Vector2 MovementDirection { get; }

		public PlayerInputActions.PlayerMainActions Player { get; }

		public void SetLayer(PlayerInputController.InputLayers     layer);
		public void SetTempLayer(PlayerInputController.InputLayers layer);
		public void ResetTempLayer();
	}
}