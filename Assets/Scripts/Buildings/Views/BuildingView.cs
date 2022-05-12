﻿using Buildings.Models;
using Installers;
using UnityEngine;
using Zenject;

namespace Buildings.Views
{
	public class BuildingView : MonoBehaviour
	{
		[Inject] protected GameSettings gameSettings;

		[SerializeField] protected BoxCollider _collider;
		[SerializeField] protected Renderer _renderer;

		public BoxCollider   Collider  => _collider;
		public Renderer      Renderer  => _renderer;
		public BuildingModel Model     => _model;
		public bool          Triggered { get; private set; }

		private Color _color;
		private Color _error;

		protected BuildingModel _model;

		private void Start()
		{
			var rendererMaterial = _renderer.material;
			_color = rendererMaterial.color;
			_error = new Color(1, 0, 0, 0.5f);
		}

		private void OnTriggerEnter(Collider other)
		{
			Triggered = true;
			var rendererMaterial = _renderer.material;
			rendererMaterial.color = _error;
		}

		private void OnTriggerExit(Collider other)
		{
			Triggered = false;
			var rendererMaterial = _renderer.material;
			rendererMaterial.color = _color;
		}

		public virtual void SetModel(BuildingModel model)
		{
			_model = model;
		}

		public virtual void FinalInstantiate() { }
	}
}