using System;
using System.IO;
using System.Linq;
using JetBrains.Annotations;
using UnityEditor;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Editor
{
	public class ToolsEditor
	{
		
		[MenuItem("Tools/Game Settings/Inventory pack models")]
		private static void FindAndSelectInventoryPacksModels()
		{
			FindAndSelectAsset<InventoryPacksModelsManagerSO>();
		}

		private static void FindAndSelectPrefab<T>() where T : Object
		{
			FindAndSelect<T>(LoadAllPrefabs);
		}

		private static void FindAndSelectAsset<T>() where T : Object
		{
			FindAndSelect<T>(LoadAllAssets);
		}

		private static void FindAndSelect<T>([NotNull]Action load) where T : Object
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
				return;
			}

			Debug.LogError($"Can't find {typeof(T)}.");
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
			         .Select(s => Directory.GetFiles(s, @"*.assets", SearchOption.TopDirectoryOnly))
			         .SelectMany(strings => strings.Select(Path.GetFileNameWithoutExtension))
			         .Distinct().ToList().ForEach(s => Resources.LoadAll(s));
		}
	}
}