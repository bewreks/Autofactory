using System.Collections.Generic;
using System.Linq;
using Electricity.Controllers;
using Electricity.ControllerStates;
using Factories;
using UniRx;

namespace Electricity
{
	public class ElectricityController : IElectricityController
	{
		private IDFactory _idFactory = new IDFactory();

		private CompositeDisposable _disposables = new CompositeDisposable();

		private List<ElectricityPoleController>                        _polesToAdd;
		private List<ElectricityPoleController>                        _polesToRemove;
		private List<GeneratorController>                              _generatorsToAdd;
		private List<GeneratorController>                              _generatorsToRemove;
		private List<BuildingPolePair>                                 _consumptionsToAdd;
		private List<BuildingPolePair>                                 _consumptionsToRemove;
		private List<GeneratorPolePair>                                _generatorsPairToAdd;
		private List<GeneratorPolePair>                                _generatorsPairToRemove;
		private Dictionary<ElectricityPoleController, List<PolesPair>> _toMerge;
		private Dictionary<ElectricityPoleController, List<PolesPair>> _toUnmerge;

		private ElectricityControllerState _state;

		private Dictionary<int, ElectricityNet_Old> _nets = new Dictionary<int, ElectricityNet_Old>();

		public ElectricityController()
		{
			_polesToAdd             = new List<ElectricityPoleController>();
			_polesToRemove          = new List<ElectricityPoleController>();
			_generatorsToAdd        = new List<GeneratorController>();
			_generatorsToRemove     = new List<GeneratorController>();
			_consumptionsToAdd      = new List<BuildingPolePair>();
			_consumptionsToRemove   = new List<BuildingPolePair>();
			_generatorsPairToAdd    = new List<GeneratorPolePair>();
			_generatorsPairToRemove = new List<GeneratorPolePair>();
			_toMerge                = new Dictionary<ElectricityPoleController, List<PolesPair>>();
			_toUnmerge              = new Dictionary<ElectricityPoleController, List<PolesPair>>();

			_state = new WaitingElectricityControllerState();
			_state.Initialize(_polesToAdd,
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

			Observable.EveryUpdate().Subscribe(l =>
			{
				while (!_state.Waiting)
				{
					_state = _state.Do(_idFactory);
				}
				// 1. Добавляем столбам без мержей новые сети
				// 2. Объединяем столбы в сеть
				// 3. Удаляем столбы без разделения на подсети
				// 4. Разделяем сети
				// 5. Добавляем новые здания в сети 
			}).AddTo(_disposables);
		}

		public void AddGenerator(GeneratorController generator)
		{
			_generatorsToAdd.Add(generator);
		}

		public void RemoveGenerator(GeneratorController generator)
		{
			_generatorsToRemove.Add(generator);
		}

		public void AddBuilding(ElectricityConsumptionBuildingController building, ElectricityPoleController pole)
		{
			_consumptionsToAdd.Add(new BuildingPolePair
			                       {
				                       Building = building,
				                       Pole     = pole
			                       });
		}

		public void RemoveBuilding(ElectricityConsumptionBuildingController building, ElectricityPoleController pole)
		{
			_consumptionsToRemove.Add(new BuildingPolePair
			                          {
				                          Building = building,
				                          Pole     = pole
			                          });
		}

		public void MergePoles(ElectricityPoleController newPole, ElectricityPoleController mainPole)
		{
			var pair = new PolesPair(mainPole, newPole);

			if (_toMerge.ContainsKey(mainPole))
			{
				pair.To   = mainPole;
				pair.From = newPole;
			}

			if (!_toMerge.TryGetValue(pair.To, out var list))
			{
				list = new List<PolesPair>();
				_toMerge.Add(pair.To, list);
			}

			list.Add(pair);
		}

		public void UnmergePoles(ElectricityPoleController newPole, ElectricityPoleController mainPole)
		{
			var pair = new PolesPair(mainPole, newPole);

			if (_toUnmerge.ContainsKey(mainPole))
			{
				pair.To   = mainPole;
				pair.From = newPole;
			}

			if (!_toUnmerge.TryGetValue(pair.To, out var list))
			{
				list = new List<PolesPair>();
				_toMerge.Add(pair.To, list);
			}

			list.Add(pair);
		}

		public void AddPole(ElectricityPoleController pole)
		{
			_polesToAdd.Add(pole);
		}

		public void RemovePole(ElectricityPoleController pole)
		{
			_polesToRemove.Add(pole);
		}

		public void AddGeneratorToNet(GeneratorController generator, ElectricityPoleController pole)
		{
			_generatorsPairToAdd.Add(new GeneratorPolePair
			                         {
				                         Generator = generator,
				                         Pole      = pole
			                         });
		}

		public void RemoveGeneratorFromNet(GeneratorController generator, ElectricityPoleController pole)
		{
			_generatorsPairToAdd.Remove(new GeneratorPolePair
			                            {
				                            Generator = generator,
				                            Pole      = pole
			                            });
		}

		public void Dispose()
		{
			_disposables?.Dispose();
		}
	}

	public class PolesPair
	{
		public ElectricityPoleController From;
		public ElectricityPoleController To;

		public ElectricityNet_Old[] Nets;

		public PolesPair(ElectricityPoleController from, ElectricityPoleController to)
		{
			From = from;
			To   = to;
			
			Nets = new[]{From.Net, To.Net}.Where(_ => _ != null).ToArray();
		}
	}

	public struct BuildingPolePair
	{
		public ElectricityPoleController                Pole;
		public ElectricityConsumptionBuildingController Building;
	}

	public struct GeneratorPolePair
	{
		public ElectricityPoleController Pole;
		public GeneratorController       Generator;
	}

	public class ElectricityNet
	{
		public void Initialize(int pop)
		{
			throw new System.NotImplementedException();
		}

		public void AddPole(ElectricityPoleController pole)
		{
			throw new System.NotImplementedException();
		}
	}
}