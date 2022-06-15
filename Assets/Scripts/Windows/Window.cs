using System;
using Installers;
using Players;
using Players.Interfaces;
using UniRx;
using UnityEngine;
using UnityEngine.InputSystem;
using Zenject;

namespace Windows
{
	public abstract class Window : MonoBehaviour
	{
		[Inject] protected IPlayerInputController _playerInputController;
		
		public event Action<Window> OnClose;

		private readonly ReactiveProperty<WindowStateEnum>
			_state = new ReactiveProperty<WindowStateEnum>(WindowStateEnum.CLOSED);

		public void Open()
		{
			if (_playerInputController != null)
				_playerInputController.Player.CloseWindows.performed += CloseWindow;
			_state.SetValueAndForceNotify(WindowStateEnum.OPENING);
			Opening();
		}

		private void CloseWindow(InputAction.CallbackContext obj)
		{
			if (_playerInputController != null)
				_playerInputController.Player.CloseWindows.performed -= CloseWindow;
			Close();
		}

		public void Close()
		{
			_state.SetValueAndForceNotify(WindowStateEnum.CLOSING);
			Closing();
		}

		protected void Opened()
		{
			_state.SetValueAndForceNotify(WindowStateEnum.OPENED);
		}

		protected void Closed()
		{
			_state.SetValueAndForceNotify(WindowStateEnum.CLOSED);
			OnClose?.Invoke(this);
			Destroy(gameObject);
		}

		protected abstract void Opening();
		protected abstract void Closing();

#if UNITY_INCLUDE_TESTS
		public ReactiveProperty<WindowStateEnum> State => _state;
#endif
	}
}