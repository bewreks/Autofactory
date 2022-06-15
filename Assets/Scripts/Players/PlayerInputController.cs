using System;
using UnityEngine;
using UnityEngine.InputSystem;
using Zenject;

namespace Players
{
	public class PlayerInputController : IDisposable
	{
		public event Action<int> FastUse;
		public event Action      CloseActiveWindow;
		public event Action      OpenCraftWindow;
		public event Action      OpenInventoryWindow;

		private PlayerInputActions _playerInputActions;
		private InputActionMap[]   _actions;
		private InputActionMap     _current;

		[Inject]
		public void Construct()
		{
			_playerInputActions               = new PlayerInputActions();
			_actions                          = new InputActionMap[2];
			_actions[(int)InputLayers.Player] = _playerInputActions.PlayerMain;
			_actions[(int)InputLayers.Window] = _playerInputActions.Window;
			SetLayer(InputLayers.Player);

			_playerInputActions.PlayerMain.FastUsing0.performed       += context => { FastUse?.Invoke(0); };
			_playerInputActions.PlayerMain.FastUsing1.performed       += context => { FastUse?.Invoke(1); };
			_playerInputActions.PlayerMain.FastUsing2.performed       += context => { FastUse?.Invoke(2); };
			_playerInputActions.PlayerMain.FastUsing3.performed       += context => { FastUse?.Invoke(3); };
			_playerInputActions.PlayerMain.FastUsing4.performed       += context => { FastUse?.Invoke(4); };
			_playerInputActions.PlayerMain.FastUsing5.performed       += context => { FastUse?.Invoke(5); };
			_playerInputActions.PlayerMain.FastUsing6.performed       += context => { FastUse?.Invoke(6); };
			_playerInputActions.PlayerMain.FastUsing7.performed       += context => { FastUse?.Invoke(7); };
			_playerInputActions.PlayerMain.FastUsing8.performed       += context => { FastUse?.Invoke(8); };
			_playerInputActions.PlayerMain.FastUsing9.performed       += context => { FastUse?.Invoke(9); };
			_playerInputActions.PlayerMain.CraftWindows.performed     += context => { OpenCraftWindow?.Invoke(); };
			_playerInputActions.PlayerMain.InventoryWindows.performed += context => { OpenInventoryWindow?.Invoke(); };
			_playerInputActions.Window.CloseWindows.performed         += context => { CloseActiveWindow?.Invoke(); };
			_playerInputActions.PlayerMain.Movement.performed         += context => { Debug.Log(context); };
		}

		public Vector2 MousePosition     => _playerInputActions.PlayerMain.Look.ReadValue<Vector2>();
		public Vector2 MovementDirection => _playerInputActions.PlayerMain.Movement.ReadValue<Vector2>();

		public void SetLayer(InputLayers layer)
		{
			_current?.Disable();
			_current = _actions[(int)layer];
			_current.Enable();
		}

		public void Dispose()
		{
			_playerInputActions?.Dispose();
			FastUse = null;
		}

		public enum InputLayers
		{
			Player = 0,
			Window = 1
		}
	}
}