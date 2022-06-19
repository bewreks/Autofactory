using System;
using System.Collections.Generic;
using Helpers;
using Installers;
using Zenject;

namespace Windows
{
	public class WindowsManager : IWindowManager
	{
		[Inject] private WindowsSettings _windowsSettings;
		[Inject] private DiContainer     _container;

		private WindowQueue _windowsQueue = new WindowQueue();
		private IWindow     _activeWindow;

		public IWindow OpenWindow<T>(Window.WindowData data, IWindowManager.WindowOpenOption option)
			where T : WindowView
		{
			var windowToOpen = _windowsSettings.GetWindow<T>();
			if (windowToOpen == null)
			{
				return default;
			}

			windowToOpen.Initialize(data, _container);

			switch (option)
			{
				case IWindowManager.WindowOpenOption.Normal:
					_windowsQueue.AddLast(windowToOpen);
					break;
				case IWindowManager.WindowOpenOption.Unique:
					if (_activeWindow != windowToOpen)
					{
						if (_activeWindow != null)
						{
							_windowsQueue.AddFirst(_activeWindow);
						}

						_windowsQueue.AddFirst(windowToOpen);

						_activeWindow?.Hide();
					}

					break;
				case IWindowManager.WindowOpenOption.QueueFirst:
					_windowsQueue.AddFirst(windowToOpen);
					break;
				default:
					throw new ArgumentOutOfRangeException(nameof(option), option, null);
			}

			OpenNextInTheQueue();

			return windowToOpen;
		}

		private void OnHideWindow(IWindow window)
		{
			_activeWindow.OnClose -= OnCloseWindow;
			_activeWindow.OnHide  -= OnHideWindow;
			_activeWindow         =  null;
			OpenNextInTheQueue();
		}

		private void OnCloseWindow(IWindow window)
		{
			_activeWindow.OnClose -= OnCloseWindow;
			_activeWindow.OnHide  -= OnHideWindow;
			_activeWindow         =  null;
			_windowsQueue.RemoveFirst();
			OpenNextInTheQueue();
		}

		public IWindow OpenWindow<T>(IWindowManager.WindowOpenOption option)
			where T : WindowView
		{
			return OpenWindow<T>(null, option);
		}

		public IWindow OpenWindow<T>(Window.WindowData data) where T : WindowView
		{
			return OpenWindow<T>(data, IWindowManager.WindowOpenOption.Normal);
		}

		private void OpenNextInTheQueue()
		{
			if (_activeWindow != null || _windowsQueue.IsEmpty)
			{
				return;
			}

			_activeWindow = _windowsQueue.First;
			_activeWindow.OnClose += OnCloseWindow;
			_activeWindow.OnHide  += OnHideWindow;
			_activeWindow.Open();
		}

		public void CloseWindow<T>()
			where T : WindowView
		{
			if (IsOpened<T>())
			{
				_activeWindow.Close();
			}
		}

		public bool IsOpened<T>()
			where T : WindowView
		{
			return _activeWindow.GetType() == typeof(T);
		}

		public void Dispose()
		{
			_windowsQueue.Clear();

			_activeWindow?.Close();
		}
	}
}