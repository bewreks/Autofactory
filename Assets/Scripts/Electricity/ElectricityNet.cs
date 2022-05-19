using System;
using System.Collections.Generic;
using System.Linq;
using Electricity.Controllers;
using ModestTree;
using UnityEngine;

namespace Electricity
{
	public class ElectricityNet : IDisposable
	{
		public int   ID    { get; private set; }
		public float Power { get; private set; }

		private List<ElectricityPoleController> _poles      = new List<ElectricityPoleController>();
		private List<GeneratorController>       _generators = new List<GeneratorController>();

		public void Dispose()
		{
			ID    = -1;
			Power = 0;
			_poles.Clear();
			_generators.ForEach(controller => controller.ChangeOfPower -= OnUpdatePower);
			_generators.Clear();
		}

		public void Initialize(int net)
		{
			ID = net;
		}

		public void AddPole(ElectricityPoleController poleController)
		{
			_poles.Add(poleController);
		}

		public bool IsPoleInWires(ElectricityPoleController newPoleController)
		{
			return _poles.Any(container =>
			{
				var distance = Vector3.Distance(container.Position, newPoleController.Position);
				return distance <= container.Wires ||
				       distance <= newPoleController.Wires;
			});
		}

		public bool RemovePole(Vector3 position, out ElectricityNet[] nets)
		{
			var firstContainer = _poles.First(container => container.Position == position);
			_poles.Remove(firstContainer);

			var tempList = new List<ElectricityNet>();

			while (!_poles.IsEmpty())
			{
				var tempNet = new ElectricityNet();
				tempNet._poles.Add(_poles[0]);
				_poles.RemoveAt(0);

				for (var i = 0; i < _poles.Count; i++)
				{
					if (tempNet.IsPoleInWires(_poles[i]))
					{
						tempNet._poles.Add(_poles[i]);
						_poles.RemoveAt(i);
						i = -1;
					}
				}

				tempList.Add(tempNet);
			}

			(_poles, tempList[0]._poles) = (tempList[0]._poles, _poles);
			tempList.RemoveAt(0);
			nets = tempList.ToArray();
			return !tempList.IsEmpty();
		}

#if UNITY_INCLUDE_TESTS
		public List<ElectricityPoleController> Poles      => _poles;
		public List<GeneratorController>       Generators => _generators;
#endif
		public void AddNet(ElectricityNet newNet)
		{
			newNet.StopUpdates();
			newNet._poles.ForEach(AddPole);
			Power += newNet.Power;
			newNet._generators.ForEach(generator =>
			{
				generator.RemoveNet(newNet);
				_generators.Add(generator);
				generator.AddNet(this);
				generator.ChangeOfPower += OnUpdatePower;
			});
			newNet.Dispose();
		}

		private void StopUpdates()
		{
			_generators.ForEach(generator => generator.ChangeOfPower -= OnUpdatePower);
		}

		public void AddGenerator(GeneratorController generator)
		{
			_generators.Add(generator);
			generator.AddNet(this);
			generator.ChangeOfPower += OnUpdatePower;
			Power += generator.PartOfPower;
		}

		private void OnUpdatePower(float oldPower, float newPower)
		{
			Debug.Log($"old {oldPower}, new {newPower}");
			Power -= oldPower;
			Power += newPower;
		}

		public bool IsBuildingInElectricity(GeneratorController generator, out ElectricityPoleController pole)
		{
			pole = null;
			return _poles.Any(pole => pole.Electricity.Overlaps(generator.BuildingRect));
		}

		public void RemoveGenerator(GeneratorController generator)
		{
			if (_generators.Contains(generator))
			{
				Power                   -= generator.PartOfPower;
				generator.ChangeOfPower -= OnUpdatePower;
				generator.RemoveNet(this);
				_generators.Remove(generator);
			}
		}
	}
}