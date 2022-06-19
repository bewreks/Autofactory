using System;

namespace Windows
{
	public interface IWindowManager : IDisposable
	{
		public IWindow OpenWindow<T>(Window.WindowData data, WindowOpenOption option) where T : WindowView;
		public IWindow OpenWindow<T>(WindowOpenOption  option) where T : WindowView;
		public IWindow OpenWindow<T>(Window.WindowData data) where T : WindowView;
		public void    CloseWindow<T>() where T : WindowView;
		public bool    IsOpened<T>() where T : WindowView;

		public enum WindowOpenOption
		{
			Normal,
			Unique,
			QueueFirst,
		}
	}
}