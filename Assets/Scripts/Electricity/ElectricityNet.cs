using System;
using System.Collections.Generic;
using Buildings.Models;
using UniRx;
using UnityEngine;

namespace Electricity
{
	public class ElectricityNet : IDisposable
	{
		public int ID { get; private set; }
		
		private List<Rect>                               _poles      = new List<Rect>();
		private List<(Rect, BaseGeneratorBuildingModel)> _generators = new List<(Rect, BaseGeneratorBuildingModel)>();

		private FloatReactiveProperty _netPower = new FloatReactiveProperty();
		public IReadOnlyReactiveProperty<float> NetPower => _netPower;

		public void Initialize(int net)
		{
			ID = net;
		}

		public void Add(ElectricityNet net)
		{
			_poles.AddRange(net._poles);
			_generators.AddRange(net._generators);
			_netPower.SetValueAndForceNotify(_netPower.Value + net._netPower.Value);
		}

		public void Add(Vector3 position, ElectricPoleBuildingModel electricBuildingModel)
		{
			var electricityHalfSize = electricBuildingModel.ElectricitySize / 2;
			_poles.Add(new Rect(new Vector2(position.x - electricityHalfSize, position.z - electricityHalfSize),
			                    new Vector2(electricBuildingModel.ElectricitySize,
			                                electricBuildingModel.ElectricitySize)));
		}

		public void Dispose()
		{
			ID = -1;
			_poles.Clear();
			_generators.Clear();
			_netPower?.Dispose();
		}

		public bool Intersect(Rect rect)
		{
			foreach (var pole in _poles)
			{
				if (pole.Overlaps(rect))
				{
					return true;
				}
			}

			return false;
		}

		public void Add(Rect position, BaseGeneratorBuildingModel model)
		{
			_netPower.SetValueAndForceNotify(_netPower.Value + model.Power);
			_generators.Add((position, model));
		}
	}
}