using System;
using System.Collections.Generic;
using Electricity.Controllers;
using Electricity.ControllerStates;
using Factories;
using Helpers;
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
			_datas.Buildings  = new List<ElectricalBuildingController>();
			_datas.Generators = new List<GeneratorController>();
			_datas.Nets       = new Dictionary<int, ElectricityNet>();

			_datas.ToMerge            = new List<PolesPair>();
			_datas.PolesToAdd         = new List<ElectricityPoleController>();
			_datas.PolesToRemove      = new List<ElectricityPoleController>();
			_datas.BuildingsToAdd     = new List<ElectricalBuildingController>();
			_datas.BuildingsToRemove  = new List<ElectricalBuildingController>();
			_datas.BuildingsPairToAdd = new Dictionary<ElectricityPoleController, List<ElectricalBuildingController>>();
			_datas.BuildingsPairToRemove =
				new Dictionary<ElectricalBuildingController, List<ElectricityPoleController>>();
			_datas.GeneratorsToAdd        = new List<GeneratorController>();
			_datas.GeneratorsToRemove     = new List<GeneratorController>();
			_datas.GeneratorsPairToAdd    = new Dictionary<ElectricityPoleController, List<GeneratorController>>();
			_datas.GeneratorsPairToRemove = new Dictionary<GeneratorController, List<ElectricityPoleController>>();

			_state = new WaitingElectricityControllerState();
			_state.Initialize(_datas);

			Observable.EveryUpdate().Subscribe(l => { _state = _state.Do(_idFactory); }).AddTo(_disposables);
		}

		public void AddGenerator(GeneratorController generator)
		{
			_datas.GeneratorsToAdd.Add(generator);
			SwitchState();
		}

		public void RemoveGenerator(GeneratorController generator)
		{
			_datas.GeneratorsToRemove.Add(generator);
			SwitchState();
		}

		public void AddBuilding(ElectricalBuildingController building)
		{
			_datas.BuildingsToAdd.AddUnique(building);
			SwitchState();
		}

		public void RemoveBuilding(ElectricalBuildingController building)
		{
			_datas.BuildingsToRemove.AddUnique(building);
			SwitchState();
		}

		public void AddBuildingToNet(ElectricalBuildingController building, ElectricityPoleController pole)
		{
			_datas.BuildingsPairToAdd.AddUnique(pole, building);
			SwitchState();
		}

		public void RemoveBuildingFromNet(ElectricalBuildingController building, ElectricityPoleController pole)
		{
			_datas.BuildingsPairToRemove.AddUnique(building, pole);
			SwitchState();
		}

		public void MergePoles(ElectricityPoleController newPole, ElectricityPoleController mainPole)
		{
			var pair = new PolesPair(mainPole, newPole);
			_datas.ToMerge.Add(pair);
			SwitchState();
		}

		public void AddPole(ElectricityPoleController pole)
		{
			_datas.PolesToAdd.AddUnique(pole);
			SwitchState();
		}

		public void RemovePole(ElectricityPoleController pole)
		{
			_datas.PolesToRemove.AddUnique(pole);
			SwitchState();
		}

		public void AddGeneratorToNet(GeneratorController generator, ElectricityPoleController pole)
		{
			_datas.GeneratorsPairToAdd.AddUnique(pole, generator);
			SwitchState();
		}

		public void RemoveGeneratorFromNet(GeneratorController generator, ElectricityPoleController pole)
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
		private ElectricityPoleController _main;
		private ElectricityPoleController _new;

		public ElectricityPoleController Main => _main;
		public ElectricityPoleController New  => _new;

		public PolesPair(ElectricityPoleController mainPole, ElectricityPoleController newPole)
		{
			_main = mainPole;
			_new  = newPole;
		}
	}

	public struct ElectricityControllerDatas : IDisposable
	{
		#region Temp data

		public List<ElectricityPoleController>                                           PolesToAdd;
		public List<ElectricityPoleController>                                           PolesToRemove;
		public List<GeneratorController>                                                 GeneratorsToAdd;
		public List<GeneratorController>                                                 GeneratorsToRemove;
		public Dictionary<ElectricityPoleController, List<GeneratorController>>          GeneratorsPairToAdd;
		public Dictionary<GeneratorController, List<ElectricityPoleController>>          GeneratorsPairToRemove;
		public List<ElectricalBuildingController>                                        BuildingsToAdd;
		public List<ElectricalBuildingController>                                        BuildingsToRemove;
		public Dictionary<ElectricityPoleController, List<ElectricalBuildingController>> BuildingsPairToAdd;
		public Dictionary<ElectricalBuildingController, List<ElectricityPoleController>> BuildingsPairToRemove;
		public List<PolesPair>                                                           ToMerge;

		#endregion

		#region Main data

		public Dictionary<int, ElectricityNet>    Nets;
		public List<GeneratorController>          Generators;
		public List<ElectricalBuildingController> Buildings;

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