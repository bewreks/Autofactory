using System;
using Buildings.Models;
using Installers;
using UnityEngine;
using Zenject;

namespace Buildings.Views
{
	public abstract class BuildingView : MonoBehaviour
	{
		[Inject] protected GameSettings gameSettings;

		[SerializeField] protected BoxCollider _collider;
		[SerializeField] protected Renderer    _renderer;
		[SerializeField] protected Transform   _bottom;

		protected abstract Type ModelType     { get; }
		protected abstract int  BuildingLayer { get; }

		public BoxCollider   Collider  => _collider;
		public Renderer      Renderer  => _renderer;
		public BuildingModel Model     => _model;
		public bool          Triggered { get; private set; }

		private Color _color;
		private Color _error;

		protected BuildingModel _model;

		private void Start()
		{
			if (!_bottom)
			{
				_bottom = transform;
			}

			var rendererMaterial = _renderer.material;
			_color = rendererMaterial.color;
			_error = new Color(1, 0, 0, 0.5f);
			gameObject.layer = gameSettings.PreviewLayer;
		}

		private void OnTriggerEnter(Collider other)
		{
			if (gameObject.layer == gameSettings.PreviewLayer)
			{
				var rendererMaterial = _renderer.material;
				rendererMaterial.color = _error;
				Triggered              = true;
			}
		}

		private void OnTriggerExit(Collider other)
		{
			if (gameObject.layer == gameSettings.PreviewLayer)
			{
				var rendererMaterial = _renderer.material;
				rendererMaterial.color = _color;
				Triggered              = false;
			}
		}

		public void SetModel(BuildingModel model)
		{
			if (model.GetType() != ModelType)
			{
				throw new NullReferenceException($"{model.name} is not {ModelType} model");
			}

			_model = model;
		}

		public void FinalInstantiate()
		{
			gameObject.layer    = BuildingLayer;
			_collider.isTrigger = false;

			OnFinalInstantiate();
		}

		protected abstract void OnFinalInstantiate();
	}
}