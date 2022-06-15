using System;
using UnityEngine;

namespace Players.Interfaces
{
	public interface IPlayerInputController : IDisposable
	{
		public Vector2 MousePosition     { get; }
		public Vector2 MovementDirection { get; }

		public PlayerInputActions.WindowsActions     WindowsActions     { get; }
		public PlayerInputActions.UsingBeforeActions UsingBeforeActions { get; }
		public PlayerInputActions.UsingAfterActions  UsingAfterActions  { get; }
	}
}