using System;
using System.Collections.Generic;
using Electricity.ControllerStates;
using Electricity.Interfaces;
using Factories;
using Helpers;
using ModestTree;
using UniRx;
using Zenject;

namespace Electricity
{
	public class ElectricityController : IElectricityController
	{
		[Inject] private DiContainer _container;

		private IDFactory _idFactory = new IDFactory();

		private CompositeDisposable _disposables = new CompositeDisposable();

		private ElectricityControllerDatas _datas = new ElectricityControllerDatas();

		private ElectricityControllerState _state;

		public ElectricityController()
		{
			_datas.Buildings  = new List<IElectricalBuildingController>();
			_datas.Generators = new List<IGeneratorController>();
			_datas.Nets       = new Dictionary<int, ElectricityNet>();

			_datas.ToMerge           = new List<PolesPair>();
			_datas.PolesToAdd        = new List<IElectricalPoleController>();
			_datas.PolesToRemove     = new List<IElectricalPoleController>();
			_datas.BuildingsToAdd    = new List<IElectricalBuildingController>();
			_datas.BuildingsToRemove = new List<IElectricalBuildingController>();
			_datas.BuildingsPairToAdd =
				new Dictionary<IElectricalPoleController, List<IElectricalBuildingController>>();
			_datas.BuildingsPairToRemove =
				new Dictionary<IElectricalBuildingController, List<IElectricalPoleController>>();
			_datas.GeneratorsToAdd        = new List<IGeneratorController>();
			_datas.GeneratorsToRemove     = new List<IGeneratorController>();
			_datas.GeneratorsPairToAdd    = new Dictionary<IElectricalPoleController, List<IGeneratorController>>();
			_datas.GeneratorsPairToRemove = new Dictionary<IGeneratorController, List<IElectricalPoleController>>();

			_state = new WaitingElectricityControllerState();
			_state.Initialize(_datas);

			Observable.EveryUpdate().Subscribe(l => { _state = _state.Do(_idFactory); }).AddTo(_disposables);
		}

		public void AddGenerator(IGeneratorController generator)
		{
			_datas.GeneratorsToAdd.Add(generator);
			SwitchState();
		}

		public void RemoveGenerator(IGeneratorController generator)
		{
			_datas.GeneratorsToRemove.AddUnique(generator);
			SwitchState();
		}

		public void AddBuilding(IElectricalBuildingController building)
		{
			_datas.BuildingsToAdd.AddUnique(building);
			SwitchState();
		}

		public void RemoveBuilding(IElectricalBuildingController building)
		{
			_datas.BuildingsToRemove.AddUnique(building);
			SwitchState();
		}

		public void AddBuildingToNet(IElectricalBuildingController building, IElectricalPoleController pole)
		{
			if (pole.Net == null)
			{
				_datas.PolesToAdd.AddUnique(pole);
			}

			_datas.BuildingsPairToAdd.AddUnique(pole, building);
			SwitchState();
		}

		public void RemoveBuildingFromNet(IElectricalBuildingController building, IElectricalPoleController pole)
		{
			_datas.BuildingsPairToRemove.AddUnique(building, pole);
			SwitchState();
		}

		public void MergePoles(IElectricalPoleController newPole, IElectricalPoleController mainPole)
		{
			if (mainPole.Net == null)
			{
				_datas.PolesToAdd.AddUnique(mainPole);
			}

			var pair = new PolesPair(mainPole, newPole);
			_datas.ToMerge.Add(pair);
			SwitchState();
		}

		public void AddPole(IElectricalPoleController pole)
		{
			if (pole.Net != null) return;
			_datas.PolesToAdd.AddUnique(pole);
			SwitchState();
		}

		public void RemovePole(IElectricalPoleController pole)
		{
			_datas.PolesToRemove.AddUnique(pole);
			SwitchState();
		}

		public void AddGeneratorToNet(IGeneratorController generator, IElectricalPoleController pole)
		{
			if (pole.Net == null)
			{
				_datas.PolesToAdd.AddUnique(pole);
			}

			_datas.GeneratorsPairToAdd.AddUnique(pole, generator);
			SwitchState();
		}

		public void RemoveGeneratorFromNet(IGeneratorController generator, IElectricalPoleController pole)
		{
			_datas.GeneratorsPairToRemove.AddUnique(generator, pole);
			SwitchState();
		}

		private void SwitchState()
		{
			if (_state is WaitingElectricityControllerState)
			{
				_state = Factory.GetFactoryItem<UpdateNetsElectricityControllerState>().Initialize(_state);
			}
		}

		public void Dispose()
		{
			_disposables?.Dispose();
			_datas.Dispose();
		}

#if UNITY_INCLUDE_TESTS
		public ElectricityControllerDatas Datas => _datas;
#endif
	}

	public class PolesPair
	{
		private IElectricalPoleController _main;
		private IElectricalPoleController _new;

		public IElectricalPoleController Main => _main;
		public IElectricalPoleController New  => _new;

		public PolesPair(IElectricalPoleController mainPole, IElectricalPoleController newPole)
		{
			_main = mainPole;
			_new  = newPole;
		}
	}

	public struct ElectricityControllerDatas : IDisposable
	{
		#region Temp data

		public List<IElectricalPoleController>                                            PolesToAdd;
		public List<IElectricalPoleController>                                            PolesToRemove;
		public List<IGeneratorController>                                                 GeneratorsToAdd;
		public List<IGeneratorController>                                                 GeneratorsToRemove;
		public Dictionary<IElectricalPoleController, List<IGeneratorController>>          GeneratorsPairToAdd;
		public Dictionary<IGeneratorController, List<IElectricalPoleController>>          GeneratorsPairToRemove;
		public List<IElectricalBuildingController>                                        BuildingsToAdd;
		public List<IElectricalBuildingController>                                        BuildingsToRemove;
		public Dictionary<IElectricalPoleController, List<IElectricalBuildingController>> BuildingsPairToAdd;
		public Dictionary<IElectricalBuildingController, List<IElectricalPoleController>> BuildingsPairToRemove;
		public List<PolesPair>                                                            ToMerge;

		#endregion

		#region Main data

		public Dictionary<int, ElectricityNet>     Nets;
		public List<IGeneratorController>          Generators;
		public List<IElectricalBuildingController> Buildings;

		#endregion

		public void Dispose()
		{
			Buildings.Clear();
			Generators.Clear();
			Nets.Clear();

			ClearTemp();
		}

		public void ClearTemp()
		{
			ToMerge.Clear();
			PolesToAdd.Clear();
			PolesToRemove.Clear();
			BuildingsToAdd.Clear();
			BuildingsToRemove.Clear();
			BuildingsPairToAdd.Clear();
			BuildingsPairToRemove.Clear();
			GeneratorsToAdd.Clear();
			GeneratorsToRemove.Clear();
			GeneratorsPairToAdd.Clear();
			GeneratorsPairToRemove.Clear();
		}
	}
}