using Players.Interfaces;
using UnityEngine;
using Zenject;

namespace Players
{
	public class PlayerInputController : IPlayerInputController
	{
		private PlayerInputActions _playerInputActions;

		public Vector2 MousePosition     { get; private set; }
		public Vector2 MovementDirection { get; private set; }

		public PlayerInputActions.WindowsActions     WindowsActions     { get; private set; }
		public PlayerInputActions.UsingBeforeActions UsingBeforeActions { get; private set; }
		public PlayerInputActions.UsingAfterActions  UsingAfterActions  { get; private set; }

		[Inject]
		public void Construct()
		{
			_playerInputActions = new PlayerInputActions();
			_playerInputActions.PlayerMove.Enable();
			_playerInputActions.Windows.Enable();
			_playerInputActions.UsingBefore.Enable();

			WindowsActions     = _playerInputActions.Windows;
			UsingAfterActions  = _playerInputActions.UsingAfter;
			UsingBeforeActions = _playerInputActions.UsingBefore;

			_playerInputActions.PlayerMove.Movement.performed += context =>
			{
				MovementDirection = context.ReadValue<Vector2>();
			};
			_playerInputActions.PlayerMove.Movement.canceled += context => { MovementDirection = Vector2.zero; };
			_playerInputActions.PlayerMove.Look.performed += context =>
			{
				MousePosition = context.ReadValue<Vector2>();
			};
		}

		public void Dispose()
		{
			_playerInputActions?.Dispose();
		}
	}
}