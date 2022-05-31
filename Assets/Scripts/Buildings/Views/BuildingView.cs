using System;
using Buildings.Interfaces;
using Buildings.Models;
using Installers;
using UniRx;
using UniRx.Triggers;
using UnityEngine;
using Zenject;

namespace Buildings.Views
{
	public abstract class BuildingView : MonoBehaviour, IDisposable
	{
		[Inject] protected GameSettings gameSettings;

		[SerializeField] protected BoxCollider _collider;
		[SerializeField] protected Renderer    _renderer;
		[SerializeField] protected Transform   _bottom;

		protected CompositeDisposable _disposables = new CompositeDisposable();

		protected abstract Type ModelType     { get; }
		protected abstract int  BuildingLayer { get; }

		public BoxCollider   Collider  => _collider;
		public Renderer      Renderer  => _renderer;
		public IBuildingModel Model     => _model;
		public bool          Triggered { get; private set; }

		private Color _color;
		private Color _error;

		protected IBuildingModel _model;

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

			_collider.OnTriggerEnterAsObservable().Subscribe(_ =>
			{
				rendererMaterial.color = _error;
				Triggered              = true;
			}).AddTo(_disposables);
			_collider.OnTriggerExitAsObservable().Subscribe(_ =>
			{
				rendererMaterial.color = _color;
				Triggered              = false;
			}).AddTo(_disposables);
		}

		public void SetModel(IBuildingModel model)
		{
			if (model.GetType() != ModelType)
			{
				throw new NullReferenceException($"{model.name} is not {ModelType} model");
			}

			_model = model;
		}

		public void FinalInstantiate()
		{
			_collider.isTrigger = false;
			gameObject.layer    = BuildingLayer;
			_disposables.Dispose();
			_disposables = new CompositeDisposable();

			OnFinalInstantiate();
		}

		public void RemoveInstance()
		{
			OnRemoveInstance();
		}

		private void OnDestroy()
		{
			Dispose();
		}

		public void Dispose()
		{
			_disposables?.Dispose();
		}

		protected abstract void OnFinalInstantiate();
		protected abstract void OnRemoveInstance();
	}
}