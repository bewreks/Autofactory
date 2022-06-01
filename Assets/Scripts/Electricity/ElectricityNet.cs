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
		private CompositeDisposable _generatorDisposables = new CompositeDisposable();

		private bool _stopPowerUpdates = true;

		public void Initialize(int netId)
		{
			ID                = netId;
			_stopPowerUpdates = false;
		}

		public void Initialize(int netId, List<IElectricalPoleController> poles)
		{
			_poles.AddRange(poles);
			_generators.AddRange(_poles.AllGenerators());
			_buildings.AddRange(_poles.AllBuildings());
			SetActualPower();

			_generators.ForEach(generator =>
			{
				generator.ActualPower
				         .PairWithPrevious()
				         .Subscribe(OnGeneratorUpdate)
				         .AddTo(_generatorDisposables);
			});
			Initialize(netId);
			StartPowerUpdates();
		}

		private void SetActualPower()
		{
			Power = _generators.Sum(generator => generator.ActualPower.Value);
		}

		private void OnGeneratorUpdate(Tuple<float, float> tuple)
		{
			if (_stopPowerUpdates) return;

			Power -= tuple.Item1;
			Power += tuple.Item2;
		}

		public void Unite(IElectricityNet net)
		{
			StopPowerUpdates();
			net.StopPowerUpdates();

			_poles.AddUniqueRange(net.Poles);
			_buildings.AddUniqueRange(net.Buildings);
			_generators.AddUniqueRange(net.Generators, out var added);
			foreach (var generator in added)
			{
				generator.ActualPower
				         .PairWithPrevious()
				         .Subscribe(OnGeneratorUpdate)
				         .AddTo(_generatorDisposables);
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
			_generatorDisposables?.Dispose();
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
			_generators.AddUnique(generator);
			generator.AddNet(this);
		}

		public void RemoveGenerator(IGeneratorController generator)
		{
			_generators.Remove(generator);
			generator.RemoveNet(this);
		}

		public void AddGenerators(List<IGeneratorController> generators)
		{
			_generators.AddUniqueRange(generators);
			generators.ForEach(generator => generator.AddNet(this));
		}

		public void RemoveGenerators(List<IGeneratorController> generators)
		{
			_generators.RemoveAll(generators.Contains);
			generators.ForEach(generator => generator.RemoveNet(this));
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