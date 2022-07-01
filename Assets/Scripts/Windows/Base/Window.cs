using System;
using Installers;
using UniRx;
using UnityEngine;
using Zenject;

namespace Windows
{
	public abstract class Window : ScriptableObject,
	                               IWindow
	{
		[SerializeField] private WindowView viewPrefab;
		[Header("window settings")]
		[SerializeField] private float openDuration = 0.25f;
		[SerializeField] private float closeDuration = 0.25f;

		[Inject] private DiContainer _diContainer;

		protected IWindowController _controller;
		protected WindowView        _view;

		public event Action<IWindow> OnOpen;
		public event Action<IWindow> OnClose;
		public event Action<IWindow> OnHide;

		public WindowData                                 Data       { get; private set; }
		public Type                                       WindowType => GetType();
		public IReadOnlyReactiveProperty<WindowStateEnum> State      => _controller.State;

		public void Initialize(WindowData data, DiContainer container)
		{
			container.Inject(this);
			Data = data;
		}

		public void Open()
		{
			Data.openWindowDuration  = openDuration;
			Data.closeWindowDuration = closeDuration;
			Data.hideWindowDuration  = closeDuration;
			_view                    = _diContainer.InstantiatePrefabForComponent<WindowView>(viewPrefab);
			_controller              = CreateWindowController();
			_diContainer.Inject(_controller);
			_controller.State.Subscribe(_ =>
			{
				switch (_)
				{
					case WindowStateEnum.OPENED:
						OnOpen?.Invoke(this);
						break;
					case WindowStateEnum.CLOSED:
						OnClose?.Invoke(this);
						Dispose();
						break;
					case WindowStateEnum.HIDDEN:
						OnHide?.Invoke(this);
						Dispose();
						break;
				}
			});
			_controller.PrepareView();
			_controller.Open();
		}

		public void Close()
		{
			_controller.Close();
		}

		public void Hide()
		{
			_controller.Hide();
		}

		public void Dispose()
		{
			_controller.Dispose();
			if (_view)
				Destroy(_view.gameObject);
		}

		protected abstract IWindowController CreateWindowController();

		[Serializable]
		public class WindowData
		{
			public float openWindowDuration;
			public float closeWindowDuration;
			public float hideWindowDuration;
		}
	}
}