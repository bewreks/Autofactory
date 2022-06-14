using System;
using Buildings.Colliders;
using Buildings.Models;
using Electricity;
using Electricity.Controllers;
using Helpers;
using Installers;
using UniRx;
using UniRx.Triggers;
using UnityEngine;
using Zenject;

namespace Buildings.Views
{
	public class ElectricPoleBuildingView : BuildingView
	{
		[Inject] private IElectricityController  _electricityController;
		[Inject] private DiContainer             _container;
		[Inject] private BuildingsModelsSettings _buildingSettings;

		private int _netID = -1;

		public int NetID => _netID;

		protected override Type                      ModelType      => typeof(ElectricalPoleModel);
		protected override int                       BuildingLayer  => gameSettings.PoleLayer;
		public             ElectricalPoleModel PoleModel      => (ElectricalPoleModel)_model;
		public             ElectricalPoleController PoleController { get; private set; }

		protected override void OnFinalInstantiate()
		{
			PoleController = new ElectricalPoleController(transform.position, PoleModel);

			var colliderView =
				_container.InstantiatePrefabForComponent<ColliderView>(_buildingSettings.SquareCollider, _bottom);
			colliderView.SetSize(PoleModel.ElectricitySize);

			colliderView.TriggerCollider.OnTriggerEnterAsObservable().Subscribe(other =>
			{
				_electricityController.AddPole(PoleController);
				var generatorBuildingView = other.GetComponent<GeneratorBuildingView>();
				if (generatorBuildingView)
				{
					_electricityController.AddGeneratorToNet(generatorBuildingView.GeneratorController, PoleController);
				}

				var consumptionBuildingView = other.GetComponent<ElectricBuildingView>();
				if (consumptionBuildingView)
				{
					_electricityController.AddBuildingToNet(consumptionBuildingView.BuildingController, PoleController);
				}
			}).AddTo(_disposables);
			colliderView.TriggerCollider.OnTriggerExitAsObservable().Subscribe(other =>
			{
				var generatorBuildingView = other.GetComponent<GeneratorBuildingView>();
				if (generatorBuildingView)
				{
					_electricityController.RemoveGeneratorFromNet(generatorBuildingView.GeneratorController, PoleController);
				}

				var consumptionBuildingView = other.GetComponent<ElectricBuildingView>();
				if (consumptionBuildingView)
				{
					_electricityController.RemoveBuildingFromNet(consumptionBuildingView.BuildingController, PoleController);
				}
			}).AddTo(_disposables);

			colliderView =
				_container.InstantiatePrefabForComponent<ColliderView>(_buildingSettings.WiresCollider, _bottom);
			colliderView.SetSize(PoleModel.WireRadius);

			colliderView.TriggerCollider.OnTriggerEnterAsObservable().Subscribe(other =>
			{
				_electricityController.AddPole(PoleController);
				var poleBuildingView = other.GetComponent<ElectricPoleBuildingView>();
				if (poleBuildingView)
				{
					_electricityController.MergePoles(poleBuildingView.PoleController, PoleController);
				}
			}).AddTo(_disposables);
			colliderView.TriggerCollider.OnTriggerExitAsObservable().Subscribe(other =>
			{
			}).AddTo(_disposables);
		}

		protected override void OnRemoveInstance()
		{
			_electricityController.RemovePole(PoleController);
		}

		private void OnDrawGizmos()
		{
			var electricModel = (ElectricalPoleModel)_model;
			var position      = transform.position;
			var rect = electricModel
				           ? BuildingHelper.GetPoleRect(position, electricModel.ElectricitySize)
				           : BuildingHelper.GetPoleRect(position, 7);

			rect.DrawGizmo(Color.blue);
			rect = electricModel
				       ? BuildingHelper.GetPoleRect(position, electricModel.WireRadius)
				       : BuildingHelper.GetPoleRect(position, 7);
			rect.DrawGizmo(Color.yellow);
		}
	}
}