using System;
using Cysharp.Threading.Tasks;
using Installers;
using Players.Interfaces;
using UniRx;
using UnityEngine.InputSystem;
using Zenject;

namespace Windows
{
	public abstract class WindowController : IWindowController
	{
		private readonly ReactiveProperty<WindowStateEnum>
			_state = new ReactiveProperty<WindowStateEnum>(WindowStateEnum.NOT_INITED);

		[Inject] protected IPlayerInputController _playerInputController;
		[Inject] protected DiContainer            _diContainer;

		protected readonly   WindowView        _view;
		protected readonly Window.WindowData _data;

		public IReadOnlyReactiveProperty<WindowStateEnum> State => _state;

		public WindowController(WindowView view, Window.WindowData data)
		{
			_view = view;
			_data = data;
		}

		public async void Open()
		{
			if (_playerInputController != null)
				_playerInputController.WindowsActions.CloseWindows.performed += CloseWindow;
			_state.SetValueAndForceNotify(WindowStateEnum.OPENING);

			_view.BeforeOpen();
			_view.OnClose  += Close;
			_view.Opening(_data.openWindowDuration);
			await UniTask.Delay(TimeSpan.FromSeconds(_data.openWindowDuration));
			_state.SetValueAndForceNotify(WindowStateEnum.OPENED);
			_view.AfterOpen();
		}

		private void CloseWindow(InputAction.CallbackContext obj)
		{
			if (_playerInputController != null)
				_playerInputController.WindowsActions.CloseWindows.performed -= CloseWindow;
			Close();
		}

		public async void Close()
		{
			_state.SetValueAndForceNotify(WindowStateEnum.CLOSING);
			_view.BeforeClose();
			_view.Closing(_data.closeWindowDuration);
			await UniTask.Delay(TimeSpan.FromSeconds(_data.closeWindowDuration));
			_state.SetValueAndForceNotify(WindowStateEnum.CLOSED);
			_view.AfterClose();
		}

		public async void Hide()
		{
			_state.SetValueAndForceNotify(WindowStateEnum.HIDING);
			_view.BeforeClose();
			_view.Hiding(_data.hideWindowDuration);
			await UniTask.Delay(TimeSpan.FromSeconds(_data.hideWindowDuration));
			_state.SetValueAndForceNotify(WindowStateEnum.HIDDEN);
			_view.AfterClose();
		}

		public virtual void Dispose()
		{
			if (_view)
				_view.OnClose -= Close;
			_state?.Dispose();
		}

		public abstract void PrepareView();
	}
}