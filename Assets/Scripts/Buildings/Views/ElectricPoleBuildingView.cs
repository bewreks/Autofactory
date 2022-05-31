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

		protected override Type                      ModelType      => typeof(ElectricPoleBuildingModel);
		protected override int                       BuildingLayer  => gameSettings.PoleLayer;
		public             ElectricPoleBuildingModel PoleModel      => (ElectricPoleBuildingModel)_model;
		public             ElectricityPoleController PoleController { get; private set; }

		protected override void OnFinalInstantiate()
		{
			PoleController = new ElectricityPoleController(transform.position, PoleModel);

			var colliderView =
				_container.InstantiatePrefabForComponent<ColliderView>(_buildingSettings.SquareCollider, _bottom);

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

			/*squareCollider.Pole = this;

			var transformCache = transform;
			var electricModel  = (ElectricPoleBuildingModel)_model;

			var hits = Physics.SphereCastAll(transformCache.position,
			                                 electricModel.WireRadius,
			                                 transformCache.forward,
			                                 electricModel.WireRadius,
			                                 gameSettings.ElectricPoleMask);

			var nets = new List<int>();

			foreach (var hit in hits.Select(hit => hit.collider.gameObject.GetComponent<ElectricPoleBuildingView>())
			                        .Where(view => view != null))
			{
				if (hit == this)
				{
					continue;
				}

				if (!nets.Contains(hit.NetID))
				{
					nets.Add(hit.NetID);
				}
			}

			if (nets.Count > 1)
			{
				_electricityController.Unite(nets);
			}

			if (nets.Count >= 1)
			{
				_electricityController.AddPole(transformCache.position, electricModel, nets.First());
			}
			else
			{
				_electricityController.AddPole(transformCache.position, electricModel);
			}*/
		}

		protected override void OnRemoveInstance()
		{
			_electricityController.RemovePole(PoleController);
		}

		private void OnDrawGizmos()
		{
			var electricModel = (ElectricPoleBuildingModel)_model;
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