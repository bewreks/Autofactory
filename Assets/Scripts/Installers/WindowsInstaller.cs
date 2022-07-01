using System;
using System.Collections.Generic;
using System.Linq;
using Windows;
using UnityEngine;
using Zenject;

namespace Installers
{
	[CreateAssetMenu(fileName = "WindowsInstaller", menuName = "Installers/WindowsInstaller")]
	public class WindowsInstaller : ScriptableObjectInstaller<WindowsInstaller>
	{
		[SerializeField] private WindowsSettings windowsSettings;

		public override void InstallBindings()
		{
			windowsSettings.Prepare();
			Container.Bind<WindowsSettings>().FromInstance(windowsSettings).AsSingle();
		}
	}

	[Serializable]
	public class WindowsSettings
	{
		[SerializeField] private List<Window> _windows = new List<Window>();

		private Dictionary<Type, IWindow> _windowsMap;

		public bool Prepare()
		{
			try
			{
				_windowsMap = _windows.ToDictionary(window => window.WindowType, window => (IWindow)window);
			}
			catch (Exception)
			{
				return false;
			}

			return true;
		}

		public IWindow GetWindow<T>()
		{
			return GetWindow(typeof(T));
		}

		private IWindow GetWindow(Type type)
		{
			if (_windowsMap == null) return null;

			_windowsMap.TryGetValue(type, out var window);
			return window;
		}

#if UNITY_INCLUDE_TESTS
		public List<Window>              Windows    => _windows;
		public Dictionary<Type, IWindow> WindowsMap => _windowsMap;
#endif
	}

	public enum WindowStateEnum
	{
		NOT_INITED,
		OPENING,
		OPENED,
		CLOSING,
		CLOSED,
		HIDING,
		HIDDEN
	}
}