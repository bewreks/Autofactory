using Players.Interfaces;
using UnityEngine;
using UnityEngine.InputSystem;
using Zenject;

namespace Players
{
	public class PlayerInputController : IPlayerInputController
	{
		private PlayerInputActions _playerInputActions;
		private InputActionMap[]   _actions;
		private InputActionMap     _current;
		private InputLayers        _currentLayer;
		private InputLayers        _tempLayer;

		public Vector2 MousePosition     { get; private set; }
		public Vector2 MovementDirection { get; private set; }

		public PlayerInputActions.PlayerMainActions Player => _playerInputActions.PlayerMain;

		[Inject]
		public void Construct()
		{
			_playerInputActions               = new PlayerInputActions();
			_actions                          = new InputActionMap[1];
			_actions[(int)InputLayers.Player] = _playerInputActions.PlayerMain;
			SetLayer(InputLayers.Player);

			_playerInputActions.PlayerMain.Movement.performed += context =>
			{
				MovementDirection = context.ReadValue<Vector2>();
			};
			_playerInputActions.PlayerMain.Movement.canceled += context => { MovementDirection = Vector2.zero; };
			_playerInputActions.PlayerMain.Look.performed += context =>
			{
				MousePosition = context.ReadValue<Vector2>();
			};
		}

		public void SetLayer(InputLayers layer)
		{
			_current?.Disable();
			_current = _actions[(int)layer];
			_current.Enable();
			_currentLayer = layer;
		}

		public void SetTempLayer(InputLayers layer)
		{
			_tempLayer = _currentLayer;
			SetLayer(layer);
		}

		public void ResetTempLayer()
		{
			SetLayer(_tempLayer);
		}

		public void Dispose()
		{
			_playerInputActions?.Dispose();
		}

		public enum InputLayers
		{
			Player = 0,
		}
	}
}