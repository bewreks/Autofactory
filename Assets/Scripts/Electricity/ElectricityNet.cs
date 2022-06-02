using System;
using System.Collections.Generic;
using System.Linq;
using Electricity.Interfaces;
using Helpers;
using UniRx;

namespace Electricity
{
	public class ElectricityNet : IElectricityNet
	{
		public int                                 ID         { get; private set; }
		public float                               Power      { get; private set; }
		public List<IGeneratorController>          Generators => _generators;
		public List<IElectricalPoleController>     Poles      => _poles;
		public List<IElectricalBuildingController> Buildings  => _buildings;


		private List<IElectricalPoleController>     _poles      = new List<IElectricalPoleController>();
		private List<IGeneratorController>          _generators = new List<IGeneratorController>();
		private List<IElectricalBuildingController> _buildings  = new List<IElectricalBuildingController>();

		private CompositeDisposable _disposables          = new CompositeDisposable();

		private Dictionary<IGeneratorController, IDisposable> _generatorDisposables =
			new Dictionary<IGeneratorController, IDisposable>();

		private bool _stopPowerUpdates = true;

#if UNITY_INCLUDE_TESTS
		public bool StopPowerUpdates_Get => _stopPowerUpdates;
#endif

		public void Initialize(int netId)
		{
			ID = netId;
			StartPowerUpdates();
		}

		public void Initialize(int netId, List<IElectricalPoleController> poles)
		{
			StopPowerUpdates();
			poles.UnitePoles(_poles);
			_poles.ForEach(controller => controller.SetNet(this));
			_generators.AddRange(_poles.AllGenerators());
			_generators.ForEach(controller => controller.AddNet(this));
			_buildings.AddRange(_poles.AllBuildings());
			_buildings.ForEach(controller => controller.AddNet(this));
			SetActualPower();

			_generators.ForEach(generator =>
			{
				generator.ActualPower
				         .PairWithPrevious()
				         .Subscribe(OnGeneratorUpdate)
				         .AddTo(_generatorDisposables, generator);
			});
			Initialize(netId);
		}

		private void SetActualPower()
		{
			Power = _generators.Sum(generator => generator.ActualPower.Value.Item2);
		}

		private void OnGeneratorUpdate(Tuple<(IGeneratorController, float), (IGeneratorController, float)> tuple)
		{
			if (_stopPowerUpdates) return;
			if (!_generators.Contains(tuple.Item1.Item1)) return;
			
			Power -= tuple.Item1.Item2;
			Power += tuple.Item2.Item2;
		}

		public void Unite(IElectricityNet net)
		{
			StopPowerUpdates();
			net.StopPowerUpdates();

			_poles.AddUniqueRange(net.Poles);
			_poles.ForEach(controller => controller.SetNet(this));
			_buildings.AddUniqueRange(net.Buildings);
			net.Buildings.ForEach(controller => controller.RemoveNet(net));
			_buildings.ForEach(controller => controller.AddNet(this));
			_generators.AddUniqueRange(net.Generators, out var added);
			net.Generators.ForEach(controller => controller.RemoveNet(net));
			_generators.ForEach(controller => controller.AddNet(this));
			foreach (var generator in added)
			{
				generator.ActualPower
				         .PairWithPrevious()
				         .Subscribe(OnGeneratorUpdate)
				         .AddTo(_generatorDisposables, generator);
			}

			net.Generators.ForEach(generator => generator.RemoveNet(net));
			SetActualPower();

			StartPowerUpdates();
		}

		public void Dispose()
		{
			ID = -1;
			_poles.Clear();
			_buildings.Clear();
			_generators.Clear();
			_disposables?.Dispose();
			foreach (var disposable in _generatorDisposables.Values)
			{
				disposable.Dispose();
			}
			_generatorDisposables.Clear();
		}

		public void AddPole(IElectricalPoleController pole)
		{
			_poles.AddUnique(pole);
			pole.SetNet(this);
		}

		public void RemovePole(IElectricalPoleController pole)
		{
			_poles.Remove(pole);
			pole.SetNet(null);
		}

		public void AddPoles(List<IElectricalPoleController> poles)
		{
			_poles.AddUniqueRange(poles);
			poles.ForEach(pole => pole.SetNet(this));
		}

		public void RemovePoles(List<IElectricalPoleController> poles)
		{
			_poles.RemoveAll(poles.Contains);
			poles.ForEach(pole => pole.SetNet(null));
		}

		public void AddGenerator(IGeneratorController generator)
		{
			generator.AddNet(this);
			generator.ActualPower
			         .PairWithPrevious()
			         .Subscribe(OnGeneratorUpdate)
			         .AddTo(_generatorDisposables, generator);
			Power += generator.ActualPower.Value.Item2;
			_generators.AddUnique(generator);
		}

		public void RemoveGenerator(IGeneratorController generator)
		{
			Power += generator.ActualPower.Value.Item2;
			_generators.Remove(generator);
			if (_generatorDisposables.TryGetValue(generator, out var disposable))
			{
				disposable.Dispose();
				_generatorDisposables.Remove(generator);
			}
			generator.RemoveNet(this);
		}

		public void AddGenerators(List<IGeneratorController> generators)
		{
			generators.ForEach(generator => generator.AddNet(this));
			foreach (var generator in generators)
			{
				generator.ActualPower
				         .PairWithPrevious()
				         .Subscribe(OnGeneratorUpdate)
				         .AddTo(_generatorDisposables, generator);
			}
			
			_generators.AddUniqueRange(generators);
			SetActualPower();
		}

		public void RemoveGenerators(List<IGeneratorController> generators)
		{
			_generators.RemoveAll(generators.Contains);
			generators.ForEach(generator =>
			{
				if (_generatorDisposables.TryGetValue(generator, out var disposable))
				{
					disposable.Dispose();
					_generatorDisposables.Remove(generator);
				}
				generator.RemoveNet(this);
			});
		}

		public void AddBuilding(IElectricalBuildingController building)
		{
			_buildings.AddUnique(building);
			building.AddNet(this);
		}

		public void RemoveBuilding(IElectricalBuildingController building)
		{
			_buildings.Remove(building);
			building.RemoveNet(this);
		}

		public void AddBuildings(List<IElectricalBuildingController> buildings)
		{
			_buildings.AddUniqueRange(buildings);
			buildings.ForEach(building => building.AddNet(this));
		}

		public void RemoveBuildings(List<IElectricalBuildingController> buildings)
		{
			_buildings.RemoveAll(buildings.Contains);
			buildings.ForEach(generator => generator.RemoveNet(this));
		}

		public void StopPowerUpdates()
		{
			_stopPowerUpdates = true;
		}

		public void StartPowerUpdates()
		{
			_stopPowerUpdates = false;
		}
	}
}