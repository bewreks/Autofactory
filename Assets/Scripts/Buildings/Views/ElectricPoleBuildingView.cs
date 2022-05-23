using System;
using System.Collections.Generic;
using System.Linq;
using Buildings.Colliders;
using Buildings.Models;
using Electricity;
using Installers;
using UnityEngine;
using Zenject;

namespace Buildings.Views
{
	public class ElectricPoleBuildingView : BuildingView
	{
		[Inject] private ElectricityController_old   _electricityController;
		[Inject] private DiContainer             _container;
		[Inject] private BuildingsModelsSettings _buildingSettings;

		private int _netID = -1;

		public int NetID => _netID;

		protected override Type ModelType     => typeof(ElectricPoleBuildingModel);
		protected override int  BuildingLayer => gameSettings.PoleLayer;

		protected override void OnFinalInstantiate()
		{
			var squareCollider =
				_container.InstantiatePrefabForComponent<ElectricitySquareCollider>(_buildingSettings.SquareCollider,
																				    _bottom);

			squareCollider.Pole = this;

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
			}
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