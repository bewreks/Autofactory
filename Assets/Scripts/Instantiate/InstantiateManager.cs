using System;
using Game;
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

		public void Dispose()
		{
		}

		public GameObject InstantiatePreview()
		{
			return _container.InstantiatePrefab(_model.SelectedPack.InstancePrefab);
		}

		public void UpdatePreviewPosition(Vector3 hit)
		{
			hit.x = SetStep(hit.x);
			hit.z = SetStep(hit.z);

			_model.InstantiablePack.transform.position = hit;
		}

		public void InstantiateFinal()
		{
			_container.InstantiatePrefab(_model.SelectedPack.InstancePrefab, 
			                             _model.InstantiablePack.transform.position,
			                             _model.InstantiablePack.transform.rotation,
			                             _model.InstantiablePack.transform.parent);
		}
	}
}