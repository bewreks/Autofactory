using Electricity;
using Electricity.Interfaces;
using Helpers;
using Installers;
using UnityEditor;
using UnityEngine;
using Zenject;

namespace Editor
{
	public class ElectricityTestWindow : ZenjectEditorWindow
	{
		private DiContainer                                          _diContainer;
		private ElectricityController                                _electricityController;
		private ElectricityEditorInstaller.ElectricityEditorSettings _electricityEditorSettings;

		private float     _heightOffset;
		private Texture2D _splitterTexture;
		private Texture2D _headerTexture;
		private int       _controlID;
		private float     _scrollPosition;

		[MenuItem("Window/ElectricityTestWindow")]
		public static ElectricityTestWindow GetOrCreateWindow()
		{
			var window = GetWindow<ElectricityTestWindow>();
			window.titleContent = new GUIContent("Electricity");
			window.minSize      = new Vector2(550, 100);
			return window;
		}

		public override void InstallBindings()
		{
			ElectricityEditorInstaller.InstallFromResource(Container);
			_electricityEditorSettings = Container.Resolve<ElectricityEditorInstaller.ElectricityEditorSettings>();
			EditorApplication.playModeStateChanged += EditorApplicationOnplayModeStateChanged;
		}

		private void EditorApplicationOnplayModeStateChanged(PlayModeStateChange obj)
		{
			if (obj == PlayModeStateChange.EnteredPlayMode)
			{
				var sceneContext = FindObjectOfType<SceneContext>();
				_diContainer           = sceneContext.Container;
				_electricityController = _diContainer.Resolve<IElectricityController>() as ElectricityController;
			}

			if (obj == PlayModeStateChange.ExitingPlayMode)
			{
				_diContainer           = null;
				_electricityController = null;
			}
		}

		private void OnDestroy()
		{
			EditorApplication.playModeStateChanged -= EditorApplicationOnplayModeStateChanged;
		}

		public float TotalWidth
		{
			get { return position.width; }
		}

		public float TotalHeight
		{
			get { return position.height; }
		}

		public Texture2D SplitterTexture
		{
			get
			{
				if (!_splitterTexture)
					_splitterTexture = CreateColorTexture(_electricityEditorSettings.splitterColor);
				return _splitterTexture;
			}
		}

		public Texture2D HeaderTexture
		{
			get
			{
				if (!_headerTexture)
					_headerTexture = CreateColorTexture(_electricityEditorSettings.headerColor);
				return _headerTexture;
			}
		}

		Texture2D CreateColorTexture(Color color)
		{
			var texture = new Texture2D(1, 1);
			texture.SetPixel(1, 1, color);
			texture.Apply();
			return texture;
		}

		private float ColumnWidth(int column, float scrollOffset)
		{
			if (column == 0)
			{
				return 40;
			}

			return (TotalWidth - 40 - scrollOffset) / 5;
		}

		public override void OnGUI()
		{
			_controlID = GUIUtility.GetControlID(FocusType.Passive);
			base.OnGUI();

			_heightOffset = 0;

			if (_electricityController == null)
			{
				DrawPlayModeError();
			}

			DrawMainStats();


			var scrollbarSize = new Vector2(GUI.skin.horizontalScrollbar.CalcSize(GUIContent.none).y,
			                                GUI.skin.verticalScrollbar.CalcSize(GUIContent.none).x);
			DrawHeader(scrollbarSize.y);

			var windowBounds = new Rect(0, 0, TotalWidth, TotalHeight);
			var netsCount    = 0;
			if (_electricityController != null)
			{
				netsCount = _electricityController.Datas.Nets.Count;
			}

			var contentRect = new Rect(0,
			                           0,
			                           TotalWidth,
			                           netsCount * _electricityEditorSettings.rowHeight);
			var viewArea = new Rect(0,
			                        _heightOffset,
			                        TotalWidth - scrollbarSize.y,
			                        TotalHeight - _heightOffset);
			var vScrRect = new Rect(windowBounds.x + viewArea.width,
			                        _heightOffset,
			                        scrollbarSize.y,
			                        viewArea.height);
			_scrollPosition = GUI.VerticalScrollbar(vScrRect,
			                                        _scrollPosition,
			                                        viewArea.height,
			                                        0,
			                                        contentRect.height);
			_heightOffset = 0;

			GUI.BeginGroup(viewArea);
			{
				contentRect.y = -_scrollPosition;

				GUI.BeginGroup(contentRect);
				{
					_electricityController?.Datas.Nets.ForEveryKey((id, net) => { DrawNetLine(net, scrollbarSize.y); });
				}
				GUI.EndGroup();
			}
			GUI.EndGroup();

			HandleEvents();
		}

		private void DrawNetLine(IElectricityNet net, float scrollOffset)
		{
			int id;
			GUI.DrawTexture(new Rect(0, _heightOffset, TotalWidth, _electricityEditorSettings.rowHeight),
			                SplitterTexture);

			var rect = new Rect(_electricityEditorSettings.splitterWidth,
			                    _heightOffset + _electricityEditorSettings.splitterWidth * 2,
			                    ColumnWidth(0, scrollOffset) - _electricityEditorSettings.splitterWidth * 2,
			                    _electricityEditorSettings.rowHeight - _electricityEditorSettings.splitterWidth * 4);
			GUI.DrawTexture(rect, HeaderTexture);
			GUI.Label(rect, $"{net.ID}", _electricityEditorSettings.tableData);

			rect = new Rect(_electricityEditorSettings.splitterWidth * 2 + rect.width + rect.x,
			                _heightOffset + _electricityEditorSettings.splitterWidth * 2,
			                ColumnWidth(1, scrollOffset) - _electricityEditorSettings.splitterWidth * 2,
			                _electricityEditorSettings.rowHeight - _electricityEditorSettings.splitterWidth * 4);
			GUI.DrawTexture(rect, HeaderTexture);
			GUI.Label(rect, $"{net.Poles.Count}", _electricityEditorSettings.tableData);

			rect = new Rect(_electricityEditorSettings.splitterWidth * 2 + rect.width + rect.x,
			                _heightOffset + _electricityEditorSettings.splitterWidth * 2,
			                ColumnWidth(2, scrollOffset) - _electricityEditorSettings.splitterWidth * 2,
			                _electricityEditorSettings.rowHeight - _electricityEditorSettings.splitterWidth * 4);
			GUI.DrawTexture(rect, HeaderTexture);
			GUI.Label(rect, $"{net.Generators.Count}", _electricityEditorSettings.tableData);

			rect = new Rect(_electricityEditorSettings.splitterWidth * 2 + rect.width + rect.x,
			                _heightOffset + _electricityEditorSettings.splitterWidth * 2,
			                ColumnWidth(3, scrollOffset) - _electricityEditorSettings.splitterWidth * 2,
			                _electricityEditorSettings.rowHeight - _electricityEditorSettings.splitterWidth * 4);
			GUI.DrawTexture(rect, HeaderTexture);
			GUI.Label(rect, $"{net.Buildings.Count}", _electricityEditorSettings.tableData);

			rect = new Rect(_electricityEditorSettings.splitterWidth * 2 + rect.width + rect.x,
			                _heightOffset + _electricityEditorSettings.splitterWidth * 2,
			                ColumnWidth(4, scrollOffset) - _electricityEditorSettings.splitterWidth * 2,
			                _electricityEditorSettings.rowHeight - _electricityEditorSettings.splitterWidth * 4);
			GUI.DrawTexture(rect, HeaderTexture);
			GUI.Label(rect, $"{net.Power}", _electricityEditorSettings.tableData);

			rect = new Rect(_electricityEditorSettings.splitterWidth * 2 + rect.width + rect.x,
			                _heightOffset + _electricityEditorSettings.splitterWidth * 2,
			                ColumnWidth(5, scrollOffset) - _electricityEditorSettings.splitterWidth * 2,
			                _electricityEditorSettings.rowHeight - _electricityEditorSettings.splitterWidth * 4);
			GUI.DrawTexture(rect, HeaderTexture);
			GUI.Label(rect, "{0}", _electricityEditorSettings.tableData);

			_heightOffset += _electricityEditorSettings.rowHeight;
		}

		private void DrawPlayModeError()
		{
			GUI.Label(new Rect(0, _heightOffset, TotalWidth, _electricityEditorSettings.rowHeight),
			          "Enter in play mode",
			          _electricityEditorSettings.notPlayMode);
			_heightOffset += _electricityEditorSettings.rowHeight;
		}

		private void DrawHeader(float scrollOffset)
		{
			GUI.DrawTexture(new Rect(0, _heightOffset, TotalWidth - scrollOffset, _electricityEditorSettings.headerHeight),
			                SplitterTexture);

			var rect = new Rect(_electricityEditorSettings.splitterWidth,
			                    _heightOffset + _electricityEditorSettings.splitterWidth * 2,
			                    ColumnWidth(0, scrollOffset) - _electricityEditorSettings.splitterWidth * 2,
			                    _electricityEditorSettings.headerHeight - _electricityEditorSettings.splitterWidth * 4);
			GUI.DrawTexture(rect, HeaderTexture);
			GUI.Label(rect, "IDs", _electricityEditorSettings.headerData);

			rect = new Rect(_electricityEditorSettings.splitterWidth * 2 + rect.width + rect.x,
			                _heightOffset + _electricityEditorSettings.splitterWidth * 2,
			                ColumnWidth(1, scrollOffset) - _electricityEditorSettings.splitterWidth * 2,
			                _electricityEditorSettings.headerHeight - _electricityEditorSettings.splitterWidth * 4);
			GUI.DrawTexture(rect, HeaderTexture);
			GUI.Label(rect, "POLES", _electricityEditorSettings.headerData);

			rect = new Rect(_electricityEditorSettings.splitterWidth * 2 + rect.width + rect.x,
			                _heightOffset + _electricityEditorSettings.splitterWidth * 2,
			                ColumnWidth(2, scrollOffset) - _electricityEditorSettings.splitterWidth * 2,
			                _electricityEditorSettings.headerHeight - _electricityEditorSettings.splitterWidth * 4);
			GUI.DrawTexture(rect, HeaderTexture);
			GUI.Label(rect, "GENS", _electricityEditorSettings.headerData);

			rect = new Rect(_electricityEditorSettings.splitterWidth * 2 + rect.width + rect.x,
			                _heightOffset + _electricityEditorSettings.splitterWidth * 2,
			                ColumnWidth(3, scrollOffset) - _electricityEditorSettings.splitterWidth * 2,
			                _electricityEditorSettings.headerHeight - _electricityEditorSettings.splitterWidth * 4);
			GUI.DrawTexture(rect, HeaderTexture);
			GUI.Label(rect, "BUILDINGS", _electricityEditorSettings.headerData);

			rect = new Rect(_electricityEditorSettings.splitterWidth * 2 + rect.width + rect.x,
			                _heightOffset + _electricityEditorSettings.splitterWidth * 2,
			                ColumnWidth(4, scrollOffset) - _electricityEditorSettings.splitterWidth * 2,
			                _electricityEditorSettings.headerHeight - _electricityEditorSettings.splitterWidth * 4);
			GUI.DrawTexture(rect, HeaderTexture);
			GUI.Label(rect, "POWER", _electricityEditorSettings.headerData);

			rect = new Rect(_electricityEditorSettings.splitterWidth * 2 + rect.width + rect.x,
			                _heightOffset + _electricityEditorSettings.splitterWidth * 2,
			                ColumnWidth(5, scrollOffset) - _electricityEditorSettings.splitterWidth * 2,
			                _electricityEditorSettings.headerHeight - _electricityEditorSettings.splitterWidth * 4);
			GUI.DrawTexture(rect, HeaderTexture);
			GUI.Label(rect, "USING POWER", _electricityEditorSettings.headerData);

			rect = new Rect(_electricityEditorSettings.splitterWidth * 2 + rect.width + rect.x,
			                _heightOffset,
			                scrollOffset,
			                _electricityEditorSettings.headerHeight);
			GUI.DrawTexture(rect, HeaderTexture);

			_heightOffset += _electricityEditorSettings.headerHeight;
		}

		void HandleEvents()
		{
			switch (Event.current.GetTypeForControl(_controlID))
			{
				case EventType.ScrollWheel:
				{
					var scrollPosition =
						_scrollPosition + Event.current.delta.y * _electricityEditorSettings.scrollSpeed;
					_scrollPosition = Mathf.Clamp(scrollPosition, 0, TotalHeight);
					break;
				}
				// case EventType.MouseDown:
				// {
				// 	_selectedPoolType = TryGetPoolTypeUnderMouse();
				// 	break;
				// }
			}
		}

		private void DrawMainStats()
		{
			GUI.DrawTexture(new Rect(0, _heightOffset, TotalWidth, _electricityEditorSettings.totalDataHeaderHeight),
			                HeaderTexture);
			GUI.Label(new Rect(0, _heightOffset, TotalWidth, _electricityEditorSettings.totalDataHeaderHeight),
			          $"Nets: {_electricityController?.Datas.Nets.Count ?? 0}, " +
			          $"Free generators: {_electricityController?.Datas.Generators.Count ?? 0}, " +
			          $"Free buildings: {_electricityController?.Datas.Buildings.Count ?? 0}",
			          _electricityEditorSettings.totalData);

			_heightOffset += _electricityEditorSettings.totalDataHeaderHeight;
		}
	}
}