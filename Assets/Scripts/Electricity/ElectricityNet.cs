using System;
using System.Collections.Generic;
using System.Linq;
using Electricity.Controllers;
using Factories;
using ModestTree;
using Zenject;

namespace Electricity
{
	public class ElectricityNet : IDisposable
	{
		[Inject] private ElectricityController_old _electricityController;
		[Inject] private DiContainer           _diContainer;
		
		public int   ID    { get; private set; }
		public float Power { get; private set; }

		private List<ElectricityPoleController> _poles      = new List<ElectricityPoleController>();
		private List<GeneratorController>       _generators = new List<GeneratorController>();

		public void Initialize(int id)
		{
			ID = id;
		}

		public void Dispose()
		{
			_poles.Clear();
			_generators.ForEach(controller => controller.ChangeOfPower -= OnUpdatePower);
			_generators.Clear();
			ID    = -1;
			Power = 0;
		}

		public void AddPole(ElectricityPoleController pole)
		{
			if (!_poles.IsEmpty())
			{
				var nearlyPoles = _poles.Where(netPole => netPole.InPoleWire(pole)).ToArray();
				if (nearlyPoles.IsEmpty()) return;

				var generators = _generators.Where(generator => generator.BuildingRect.Overlaps(pole.Electricity)).ToArray();
				pole.AddPoles(nearlyPoles);
				pole.AddGenerators(generators);
			}
			pole.SetNet(this);
			_poles.Add(pole);
		}

		public bool RemovePole(ElectricityPoleController pole, out ElectricityNet[] nets)
		{
			nets = Array.Empty<ElectricityNet>();
			_poles.Remove(pole);
			pole.RemoveSelfFromNearlyPoles();
			pole.RemoveSelfFromNearlyGenerators();
			_electricityController.ReturnGenerators(pole.NearlyGenerators
			                                            .Where(generator => generator.NearlyPoles.IsEmpty()));

			if (_poles.IsEmpty())
			{
				Dispose();
				return false;
			}
			
			if (pole.NearlyPoles.Count <= 1)
			{
				return false;
			}

			var list = new List<List<ElectricityPoleController>>();
			var i    = -1;
			foreach (var nearlyPole in pole.NearlyPoles.Where(nearlyPole => i < 0 || !list[i].Contains(nearlyPole)))
			{
				list.Add(new List<ElectricityPoleController>());
				list[++i].Add(nearlyPole);
				nearlyPole.NearlyPoles.UnitePoles(list[i]);
			}
			
			_poles.Clear();
			_poles.AddRange(list[0]);
			foreach (var notInNetGenerator in _generators.Except(list[i].SelectMany(controller => controller.NearlyGenerators)
			                                                            .Distinct()))
			{
				notInNetGenerator.ChangeOfPower -= OnUpdatePower;
				Power                           -= notInNetGenerator.PartOfPower;
				notInNetGenerator.RemoveNet(this);
			}
			_generators.Clear();
			_generators.AddRange(list[i].SelectMany(controller => controller.NearlyGenerators).Distinct());

			var newNets = new List<ElectricityNet>();
			for (i = 1; i < list.Count; i++)
			{
				var net = Factory.GetFactoryItem<ElectricityNet>(_diContainer);
				net._poles.AddRange(list[i]);
				net._generators.AddRange(list[i].SelectMany(controller => controller.NearlyGenerators).Distinct());
				net.Recalculate();
				newNets.Add(net);
			}

			nets = newNets.ToArray();
			return !nets.IsEmpty();
		}

		public void AddGenerator(GeneratorController generator)
		{
			_generators.Add(generator);
			generator.AddNet(this);
			Power                   += generator.PartOfPower;
			generator.ChangeOfPower += OnUpdatePower;
			
			foreach (var pole in _poles.Where(pole => pole.Electricity.Overlaps(generator.BuildingRect)))
			{
				pole.AddGenerator(generator);
				generator.AddPole(pole);
			}
		}

		public void RemoveGenerator(GeneratorController generator)
		{
			_generators.Remove(generator);
			generator.ChangeOfPower -= OnUpdatePower;
			Power                   -= generator.PartOfPower;
			generator.RemoveNet(this);
			
			foreach (var pole in generator.NearlyPoles.Where(pole => pole.Net.ID == ID).ToArray())
			{
				pole.RemoveGenerator(generator);
				generator.NearlyPoles.Remove(pole);
			}
		}
		
		public bool IsPoleInWires(ElectricityPoleController newPoleController)
		{
			return _poles.Any(container => container.InPoleWire(newPoleController));
		}
	
		public void AddNet(ElectricityNet newNet)
		{
			Power += newNet.Power;
			newNet.StopUpdates();
			
			foreach (var newPole in newNet._poles)
			{
				foreach (var pole in _poles.Where(pole => pole.InPoleWire(newPole)))
				{
					pole.AddPole(newPole);
					newPole.AddPole(pole);
				}
			}
			_poles.AddRange(newNet._poles);

			newNet._generators.ForEach(generator =>
			{
				generator.RemoveNet(newNet);
				if (!_generators.Contains(generator))
				{
					_generators.Add(generator);
				}

				if (!generator.Nets.Contains(this))
				{
					generator.AddNet(this);
				}

				generator.ChangeOfPower += OnUpdatePower;
			});
			newNet.Dispose();
		}
		public bool IsBuildingInElectricity(BuildingController building, out object o)
		{
			o = null;
			return _poles.Any(pole => pole.Electricity.Overlaps(building.BuildingRect));
		}

		private void Recalculate()
		{
			_poles.ForEach(pole => pole.SetNet(this));
			_generators.ForEach(generator =>
			{
				generator.AddNet(this);
				generator.ChangeOfPower += OnUpdatePower;
				Power                   += generator.PartOfPower;
			});
		}

		private void OnUpdatePower(float oldPower, float newPower)
		{
			Power -= oldPower;
			Power += newPower;
		}
		
		private void StopUpdates()
		{
			_generators.ForEach(generator => generator.ChangeOfPower -= OnUpdatePower);
		}
		
#if UNITY_INCLUDE_TESTS
		public List<ElectricityPoleController> Poles      => _poles;
		public List<GeneratorController>       Generators => _generators;
#endif
	}
}