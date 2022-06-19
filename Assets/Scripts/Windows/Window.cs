using System;
using Installers;
using Players.Interfaces;
using UniRx;
using UnityEngine;
using UnityEngine.InputSystem;
using Zenject;

namespace Windows
{
	[CreateAssetMenu(fileName = "Window", menuName = "Models/Windows/Window", order = 0)]
	public sealed class Window : ScriptableObject, IWindow
	{
		[SerializeField] private WindowView viewPrefab;

		[Inject] private IPlayerInputController _playerInputController;
		[Inject] private DiContainer            _diContainer;

		private readonly ReactiveProperty<WindowStateEnum>
			_state = new ReactiveProperty<WindowStateEnum>(WindowStateEnum.CLOSED);

		private WindowView _view;

		public event Action<IWindow> OnOpen;
		public event Action<IWindow> OnClose;
		public event Action<IWindow> OnHide;

		public WindowData Data       { get; private set; }
		
		public Type       WindowType => viewPrefab.GetType();

		public void Initialize(WindowData data, DiContainer container)
		{
			container.Inject(this);
			Data = data;
		}

		public void Open()
		{
			if (_playerInputController != null)
				_playerInputController.WindowsActions.CloseWindows.performed += CloseWindow;
			_state.SetValueAndForceNotify(WindowStateEnum.OPENING);
			_view          =  _diContainer.InstantiatePrefabForComponent<WindowView>(viewPrefab);
			_view.OnClose  += Close;
			_view.OnClosed += Closed;
			_view.OnHided  += Hided;
			_view.OnOpened += Opened;
			_view.Opening();
		}

		private void CloseWindow(InputAction.CallbackContext obj)
		{
			if (_playerInputController != null)
				_playerInputController.WindowsActions.CloseWindows.performed -= CloseWindow;
			Close();
		}

		public void Close()
		{
			_state.SetValueAndForceNotify(WindowStateEnum.CLOSING);
			_view.Closing();
		}

		public void Hide()
		{
			_state.SetValueAndForceNotify(WindowStateEnum.CLOSING);
			_view.Hiding();
		}

		private void Opened()
		{
			_state.SetValueAndForceNotify(WindowStateEnum.OPENED);
			OnOpen?.Invoke(this);
		}

		private void Closed()
		{
			_state.SetValueAndForceNotify(WindowStateEnum.CLOSED);
			OnClose?.Invoke(this);
			Dispose();
		}

		private void Hided()
		{
			_state.SetValueAndForceNotify(WindowStateEnum.CLOSED);
			OnHide?.Invoke(this);
			Dispose();
		}

		public void Dispose()
		{
			_view.OnClose  -= Close;
			_view.OnClosed -= Closed;
			_view.OnHided  -= Hided;
			_view.OnOpened -= Opened;
			Destroy(_view.gameObject);
		}

#if UNITY_INCLUDE_TESTS
		public IReadOnlyReactiveProperty<WindowStateEnum> State => _state;
#endif

		[Serializable]
		public class WindowData { }
	}
}