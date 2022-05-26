using System.Collections.Generic;
using Electricity.Controllers;
using Factories;
using ModestTree;

namespace Electricity.ControllerStates
{
	public abstract class ElectricityControllerState
	{
		protected List<ElectricityPoleController>                        _polesToAdd;
		protected List<ElectricityPoleController>                        _polesToRemove;
		protected List<GeneratorController>                              _generatorsToAdd;
		protected List<GeneratorController>                              _generatorsToRemove;
		protected List<BuildingPolePair>                                 _consumptionsToAdd;
		protected List<BuildingPolePair>                                 _consumptionsToRemove;
		protected List<GeneratorPolePair>                                _generatorsPairToAdd;
		protected List<GeneratorPolePair>                                _generatorsPairToRemove;
		protected Dictionary<ElectricityPoleController, List<PolesPair>> _toMerge;
		protected Dictionary<ElectricityPoleController, List<PolesPair>> _toUnmerge;
		protected Dictionary<int, ElectricityNet_Old>                    _nets;

		public virtual bool Waiting => false;

		public void Initialize(List<ElectricityPoleController>                        polesToAdd,
		                       List<ElectricityPoleController>                        polesToRemove,
		                       List<GeneratorController>                              generatorsToAdd,
		                       List<GeneratorController>                              generatorsToRemove,
		                       List<BuildingPolePair>                                 consumptionsToAdd,
		                       List<BuildingPolePair>                                 consumptionsToRemove,
		                       List<GeneratorPolePair>                                generatorsPairToAdd,
		                       List<GeneratorPolePair>                                generatorsPairToRemove,
		                       Dictionary<ElectricityPoleController, List<PolesPair>> toMerge,
		                       Dictionary<ElectricityPoleController, List<PolesPair>> toUnmerge,
		                       Dictionary<int, ElectricityNet_Old>                    nets)
		{
			_nets                   = nets;
			_toMerge                = toMerge;
			_toUnmerge              = toUnmerge;
			_polesToAdd             = polesToAdd;
			_polesToRemove          = polesToRemove;
			_generatorsToAdd        = generatorsToAdd;
			_consumptionsToAdd      = consumptionsToAdd;
			_generatorsToRemove     = generatorsToRemove;
			_generatorsPairToAdd    = generatorsPairToAdd;
			_consumptionsToRemove   = consumptionsToRemove;
			_generatorsPairToRemove = generatorsPairToRemove;
		}

		public ElectricityControllerState Initialize(ElectricityControllerState prevState)
		{
			Initialize(_polesToAdd,
			           _polesToRemove,
			           _generatorsToAdd,
			           _generatorsToRemove,
			           _consumptionsToAdd,
			           _consumptionsToRemove,
			           _generatorsPairToAdd,
			           _generatorsPairToRemove,
			           _toMerge,
			           _toUnmerge,
			           _nets);
			return this;
		}

		public ElectricityControllerState NextState()
		{
			Factory.ReturnItem(this);
			if (!_polesToAdd.IsEmpty())
			{
				return Factory.GetFactoryItem<AddPolesElectricityControllerState>().Initialize(this);
			}

			if (!_toMerge.IsEmpty())
			{
				return Factory.GetFactoryItem<MergePolesElectricityControllerState>().Initialize(this);
			}

			if (!_polesToRemove.IsEmpty())
			{
				return Factory.GetFactoryItem<RemovePolesElectricityControllerState>().Initialize(this);
			}

			if (!_toUnmerge.IsEmpty())
			{
				return Factory.GetFactoryItem<UnmergePolesElectricityControllerState>().Initialize(this);
			}

			if (!_generatorsToAdd.IsEmpty() ||
			    !_consumptionsToAdd.IsEmpty() ||
			    !_generatorsPairToAdd.IsEmpty())
			{
				return Factory.GetFactoryItem<AddBuildingsElectricityControllerState>().Initialize(this);
			}

			if (!_generatorsToRemove.IsEmpty() ||
			    !_consumptionsToRemove.IsEmpty() ||
			    !_generatorsPairToRemove.IsEmpty())
			{
				return Factory.GetFactoryItem<RemoveBuildingsElectricityControllerState>().Initialize(this);
			}

			return Factory.GetFactoryItem<WaitingElectricityControllerState>().Initialize(this);
		}

		public abstract ElectricityControllerState Do(IDFactory idFactory);
	}
}