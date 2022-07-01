using System;
using Installers;
using UniRx;

namespace Windows
{
	public interface IWindowController : IDisposable
	{
		IReadOnlyReactiveProperty<WindowStateEnum> State { get; }

		void PrepareView();
		void Open();
		void Close();
		void Hide();
	}
}