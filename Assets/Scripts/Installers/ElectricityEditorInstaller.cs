using System;
using UnityEngine;
using Zenject;

namespace Installers
{
	[CreateAssetMenu(fileName = "ElectricityEditorInstaller", menuName = "Installers/ElectricityEditorInstaller")]
	public class ElectricityEditorInstaller : ScriptableObjectInstaller<ElectricityEditorInstaller>
	{
		[SerializeField] private ElectricityEditorSettings _settings;

		public override void InstallBindings()
		{
			Container.BindInstance(_settings);
		}

		[Serializable]
		public class ElectricityEditorSettings
		{
			[Header("Styles")]
			public GUIStyle notPlayMode;
			public GUIStyle totalData;
			public GUIStyle headerData;
			public GUIStyle tableData;

			[Header("colors")]
			public Color headerColor;
			public Color splitterColor;

			[Header("settings")]
			public float rowHeight             = 25;
			public float totalDataHeaderHeight = 32;
			public float headerHeight          = 32;
			public float splitterWidth         = 2;
			public float scrollSpeed           = 10;
		}
	}
}