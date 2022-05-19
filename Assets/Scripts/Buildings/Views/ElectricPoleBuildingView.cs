using System;
using System.Collections.Generic;
using System.Linq;
using Buildings.Models;
using Electricity;
using UnityEngine;
using Zenject;

namespace Buildings.Views
{
	public class ElectricPoleBuildingView : BuildingView
	{
		[Inject] private ElectricityController _electricityController;

		private int _netID = -1;

		public int NetID => _netID;

		public override void SetModel(BuildingModel model)
		{
			base.SetModel(model);

			if (!(_model is ElectricPoleBuildingModel))
			{
				throw new NullReferenceException($"{model.name} is not electric model");
			}
		}

		public override void FinalInstantiate()
		{
			var transformCache = transform;
			var electricModel  = (ElectricPoleBuildingModel)_model;

			var hits = Physics.SphereCastAll(transformCache.position,
			                                 electricModel.WireRadius,
			                                 transformCache.forward,
			                                 electricModel.WireRadius,
			                                 gameSettings.ElectricBuildingMask);

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