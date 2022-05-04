using System;
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
			if (coordX < 0)
			{
				coordX -= 0.5f;
			}

			return (int)(coordX / 0.5) * 0.5f;
		}

		public void Dispose() { }

		public BuildingView InstantiatePreview()
		{
			var hit = _model.MousePosition;
			hit.x = SetStep(hit.x);
			hit.z = SetStep(hit.z);

			return _container.InstantiatePrefabForComponent<BuildingView>(_model.SelectedPack.Model.Instance, hit,
			                                                              Quaternion.identity, null);
		}

		public void UpdatePreviewPosition(Vector3 hit)
		{
			hit.x = SetStep(hit.x);
			hit.z = SetStep(hit.z);

			_model.InstantiablePack.transform.position = hit;
		}

		public void InstantiateFinal()
		{
			_model.SelectedPack.Remove();
			var transform = _model.InstantiablePack.transform;
			var view = _container.InstantiatePrefabForComponent<BuildingView>(_model.SelectedPack.Model.Instance,
			                                                                  transform.position,
			                                                                  transform.rotation,
			                                                                  transform.parent);
			view.Collider.isTrigger = false;
		}
	}
}