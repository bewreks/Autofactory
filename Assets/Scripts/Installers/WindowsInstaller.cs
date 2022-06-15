using System;
using System.Collections.Generic;
using System.Linq;
using Windows;
using ModestTree;
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

	public class WindowsManager : IDisposable
	{
		[Inject] private WindowsSettings _windowsSettings;
		[Inject] private DiContainer     _container;

		private Dictionary<Type, Window> _openedWindows = new Dictionary<Type, Window>();

		public T OpenWindow<T>()
			where T : Window
		{
			if (IsOpened<T>()) return null;
			
			var windowPrefab = _windowsSettings.GetWindow<T>();
			if (windowPrefab != null)
			{
				var window = _container.InstantiatePrefab(windowPrefab).GetComponent<T>();
				window.OnClose += _ =>
				{
					_openedWindows.Remove(_.GetType());
				};
				window.Open();
				_openedWindows.Add(window.GetType(), window);
				return window;
			}
			else
			{
				return null;
			}
		}

		public bool IsOpened<T>()
		{
			return _openedWindows.ContainsKey(typeof(T));
		}

		public bool IsOpened()
		{
			return !_openedWindows.IsEmpty();
		}

		public void Dispose()
		{
			foreach (var window in _openedWindows.Values.ToArray())
			{
				window.Close();
			}
			_openedWindows.Clear();
		}
	}

	[Serializable]
	public class WindowsSettings
	{
		[SerializeField] private List<Window> _windows = new List<Window>();

		private Dictionary<Type, Window> _windowsMap;

		public bool Prepare()
		{
			try
			{
				_windowsMap = _windows.ToDictionary(window => window.GetType(), window => window);
			}
			catch (Exception)
			{
				return false;
			}

			return true;
		}

		public Window GetWindow<T>()
		{
			return GetWindow(typeof(T));
		}

		private Window GetWindow(Type type)
		{
			if (_windowsMap == null) return null;
			
			_windowsMap.TryGetValue(type, out var window);
			return window;
		}
		
#if UNITY_INCLUDE_TESTS
		public List<Window>             Windows    => _windows;
		public Dictionary<Type, Window> WindowsMap => _windowsMap;
#endif
	}

	public enum WindowStateEnum
	{
		OPENING,
		OPENED,
		CLOSING,
		CLOSED
	}
}