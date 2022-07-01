using System;
using Crafting;
using Installers;
using Inventories;
using UnityEngine;
using Zenject;

namespace Windows.CraftingWindow
{
	[CreateAssetMenu(fileName = "CraftingWindow", menuName = "Models/Windows/CraftingWindow")]
	public class CraftingWindow : Window
	{
		protected override IWindowController CreateWindowController()
		{
			return new CraftingWindowController(_view, Data);
		}
	}

	internal class CraftingWindowController : WindowController
	{
		[Inject] private CraftSettings      _craftSettings;
		[Inject] private CraftingController _craftingController;

		private InventoryPackModel _lastModel;
		private CraftingWindowView view;
		private CraftingWindowData data;

		public CraftingWindowController(WindowView view, Window.WindowData data) : base(view, data)
		{
			this.view      = (CraftingWindowView)view;
			this.data = (CraftingWindowData)_data;
		}

		public override void PrepareView()
		{
			view.OnMouseOver += OnMouseOver;
			view.OnMouseExit += OnMouseExit;
			view.OnMouseClick += OnMouseClick;
			view.CraftPackPrefab.gameObject.SetActive(true);
			view.ShowModels(_craftSettings.Models, view.CraftPackPrefab);
			view.CraftPackPrefab.gameObject.SetActive(false);
		}

		private void OnMouseClick()
		{
			_craftingController.StartCraft(data.InventoryFrom,
			                               data.InventoryTo,
			                               _lastModel.Type);
		}

		private void OnMouseExit()
		{
			view.HideHint();
		}

		private void OnMouseOver(InventoryPackModel model, RectTransform position)
		{
			_lastModel = model;
			view.ShowHint(_craftSettings.GetModel(model.Type), position);
		}

		public override void Dispose()
		{
			view.OnMouseOver  -= OnMouseOver;
			view.OnMouseExit  -= OnMouseExit;
			view.OnMouseClick -= OnMouseClick;
			base.Dispose();
		}
	}

	public class CraftingWindowData : Window.WindowData
	{
		public IInventory InventoryFrom;
		public IInventory InventoryTo;
	}
}