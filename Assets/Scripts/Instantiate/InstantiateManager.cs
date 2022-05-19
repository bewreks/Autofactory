using System;
using Buildings;
using Buildings.Views;
using Game;
using Inventories;
using UnityEngine;
using Zenject;

namespace Instantiate
{
	public class InstantiateManager : IDisposable
	{
		[Inject] private IGameModel  _model;
		[Inject] private DiContainer _container;

		private float SetStep(float coordX)
		{
			return (int)coordX;
		}

		public void Dispose() { }

		public BuildingView InstantiatePreview()
		{
			var hit = _model.MousePosition;
			hit.x = SetStep(hit.x);
			hit.z = SetStep(hit.z);

			var preview =
				_container.InstantiatePrefabForComponent<BuildingView>(_model.SelectedPack.Model.BuildingModel.Instance,
				                                                       hit,
				                                                       Quaternion.identity,
				                                                       null);
			preview.SetModel(_model.SelectedPack.Model.BuildingModel);
			return preview;
		}

		public void UpdatePreviewPosition(Vector3 hit)
		{
			hit.x = SetStep(hit.x);
			hit.z = SetStep(hit.z);

			_model.InstantiablePack.transform.position = hit;
		}

		public void InstantiateFinal()
		{
			//TODO проверить удаление
			_model.SelectedPack.Remove();
			var view = _model.InstantiablePack;
			view.FinalInstantiate();
			view.Collider.isTrigger = false;
		}
	}
}