using System;
using System.IO;
using System.Linq;
using Windows;
using Inventories;
using JetBrains.Annotations;
using UnityEditor;
using UnityEngine;
using Object = UnityEngine.Object;

#if UNITY_EDITOR
namespace Editor
{
	public class ToolsEditor
	{
		[MenuItem("Tools/Game Settings/Inventory pack models")]
		private static void FindAndSelectInventoryPacksModels()
		{
			FindAndSelectAsset<InventoryPacksInstaller>();
		}

		[MenuItem("Tools/Game Settings/Windows settings")]
		private static void FindAndSelectWindowsSettings()
		{
			FindAndSelectAsset<WindowsInstaller>();
		}

		public static T FindAndSelectPrefab<T>() where T : Object
		{
			return FindAndSelect<T>(LoadAllPrefabs);
		}

		public static T FindAndSelectAsset<T>() where T : Object
		{
			return FindAndSelect<T>(LoadAllAssets);
		}

		private static T FindAndSelect<T>([NotNull] Action load) where T : Object
		{
			var instance = Resources.FindObjectsOfTypeAll<T>().FirstOrDefault();
			if (!instance)
			{
				load();
				instance = Resources.FindObjectsOfTypeAll<T>().FirstOrDefault();
			}

			if (instance)
			{
				Selection.activeObject = instance;
				return instance;
			}

			Debug.LogError($"Can't find {typeof(T)}.");
			return default;
		}

		private static void LoadAllPrefabs()
		{
			Directory.GetDirectories(Application.dataPath, @"Resources", SearchOption.AllDirectories)
			         .Select(s => Directory.GetFiles(s, @"*.prefab", SearchOption.TopDirectoryOnly))
			         .SelectMany(strings => strings.Select(Path.GetFileNameWithoutExtension))
			         .Distinct().ToList().ForEach(s => Resources.LoadAll(s));
		}

		private static void LoadAllAssets()
		{
			Directory.GetDirectories(Application.dataPath, @"Resources", SearchOption.AllDirectories)
			         .Select(s => Directory.GetFiles(s, @"*.asset", SearchOption.AllDirectories))
			         .SelectMany(strings => strings.Select(Path.GetFileNameWithoutExtension))
			         .Distinct().ToList().ForEach(s => Resources.LoadAll(s));
		}
	}
}
#endif