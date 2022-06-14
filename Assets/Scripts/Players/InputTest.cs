using Players;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputTest : MonoBehaviour
{
	private PlayerInputActions _playerInputActions;

	private void Awake()
	{
		_playerInputActions = new PlayerInputActions();
		_playerInputActions.Player.Enable();
		_playerInputActions.Window.Disable();

		// _playerInputActions.Player.FastUsing0.performed += OnEye;
		// _playerInputActions.Player.FastUsing1.performed += OnEye;
		// _playerInputActions.Player.FastUsing2.performed += OnEye;
		// _playerInputActions.Player.FastUsing3.performed += OnEye;
		// _playerInputActions.Player.FastUsing4.performed += OnEye;
		// _playerInputActions.Player.FastUsing5.performed += OnEye;
		// _playerInputActions.Player.FastUsing6.performed += OnEye;
		// _playerInputActions.Player.FastUsing7.performed += OnEye;
		// _playerInputActions.Player.FastUsing8.performed += OnEye;
		// _playerInputActions.Player.FastUsing9.performed += OnEye;
		//
		// _playerInputActions.Window.CloseWindows.performed += OnEye1;

		_playerInputActions.Player.Look.performed += OnEye;
		// _playerInputActions.Player.Movement.performed += OnEye;

	}

	public void OnEye1(InputAction.CallbackContext context)
	{
		Debug.Log(context);
		_playerInputActions.Window.Disable();
		_playerInputActions.Player.Enable();
	}

	public void OnEye(InputAction.CallbackContext context)
	{
		
		Debug.Log(_playerInputActions.Player.Look.ReadValue<Vector2>());
		Debug.Log(context);
		// _playerInputActions.Window.Enable();
		// _playerInputActions.Player.Disable();
	}
}