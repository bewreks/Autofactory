using System;

namespace Windows
{
	public interface IWindowManager : IDisposable
	{
		public IWindow OpenWindow<T>(Window.WindowData data, WindowOpenOption option) where T : IWindow;
		public IWindow OpenWindow<T>(WindowOpenOption  option) where T : IWindow;
		public IWindow OpenWindow<T>(Window.WindowData data) where T : IWindow;
		public void    CloseWindow<T>() where T : IWindow;
		public bool    IsOpened<T>() where T : IWindow;

		public enum WindowOpenOption
		{
			Normal,
			Unique,
			QueueFirst,
		}
	}
}