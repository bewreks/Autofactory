﻿using System;
using Installers;
using Zenject;

namespace Windows
{
	public interface IWindow : IDisposable
	{
		public event Action<IWindow> OnOpen;
		public event Action<IWindow> OnClose;
		public event Action<IWindow> OnHide;

		public Window.WindowData Data          { get; }
		public Type              WindowType    { get; }
		public WindowStateEnum   State         { get; }
		public float             OpenDuration  { get; }
		public float             CloseDuration { get; }

		public void              Initialize(Window.WindowData data, DiContainer container);

		public void Open();
		public void Close();
		public void Hide();
	}
}