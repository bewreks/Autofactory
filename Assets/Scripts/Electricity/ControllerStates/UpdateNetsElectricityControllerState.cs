using System.Collections.Generic;
using System.Linq;
using Electricity.Interfaces;
using Factories;
using Helpers;
using ModestTree;
using UnityEngine;

namespace Electricity.ControllerStates
{
	public class UpdateNetsElectricityControllerState : ElectricityControllerState
	{
		public override ElectricityControllerState Do(IDFactory idFactory)
		{
			foreach (var pole in _datas.PolesToAdd)
			{
				var newNet = CreateNewNet(idFactory);
				newNet.AddPole(pole);
			}

			var toRemove = new List<IEnumerable<IElectricalPoleController>>();
			foreach (var pole in _datas.PolesToRemove)
			{
				toRemove.Add(pole.NearlyPoles.ToList());
				foreach (var nearlyPole in pole.NearlyPoles)
				{
					nearlyPole.RemovePole(pole);
				}
				pole.RemovePoles(pole.NearlyPoles);
				foreach (var generator in pole.NearlyGenerators)
				{
					_datas.GeneratorsPairToRemove.AddUnique(generator, pole);	
				}
				foreach (var building in pole.NearlyBuildings)
				{
					_datas.BuildingsPairToRemove.AddUnique(building, pole);
				}
			}

			foreach (var neighbours in toRemove)
			{
				var list = new List<List<IElectricalPoleController>>();
				var i    = -1;
				foreach (var nearlyPole in neighbours.Where(nearlyPole => i < 0 ||
				                                                          !list[i].Contains(nearlyPole)))
				{
					list.Add(new List<IElectricalPoleController>());
					list[++i].Add(nearlyPole);
					nearlyPole.NearlyPoles.UnitePoles(list[i]);
				}

				if (list.Count <= 1)
				{
					continue;
				}


				var net   = list[0][0].Net;
				var netID = net.ID;
				net.Dispose();
				net.Initialize(netID, list[0]);

				for (i = 1; i < list.Count; i++)
				{
					CreateNewNet(idFactory, list[i]);
				}
			}

			foreach (var pair in _datas.ToMerge)
			{
				pair.Main.AddPole(pair.New);
				pair.New.AddPole(pair.Main);
				
				if (pair.Main.Net == pair.New.Net && pair.Main.Net != null) continue;

				switch (pair.Main.Net)
				{
					case null when pair.New.Net == null:
					{
						var net = CreateNewNet(idFactory);
						net.AddPole(pair.Main);
						net.AddPole(pair.New);

						_datas.Nets.Add(net.ID, net);
						break;
					}
					case null when pair.New.Net != null:
						pair.New.Net.AddPole(pair.Main);
						break;
					default:
					{
						if (pair.Main.Net != null && pair.New.Net == null)
						{
							pair.Main.Net.AddPole(pair.New);
						}
						else
						{
							var to = _datas.Nets[Mathf.Min(pair.Main.Net!.ID,
							                               pair.New.Net.ID)];
							var from = _datas.Nets[Mathf.Max(pair.Main.Net.ID,
							                                 pair.New.Net.ID)];
							to.Unite(from);
							_datas.Nets.Remove(from.ID);
							idFactory.Push(from.ID);
							from.Dispose();
							Factory.ReturnItem(from);
						}

						break;
					}
				}
			}

			_datas.Generators.AddUniqueRange(_datas.GeneratorsToAdd);
			_datas.Buildings.AddUniqueRange(_datas.BuildingsToAdd);

			_datas.GeneratorsPairToAdd.ForEveryKey((pole, generators) =>
			{
				pole.Net.AddGenerators(generators);
				pole.AddGenerators(generators);
				generators.ForEach(generator => generator.AddPole(pole));
			});
			_datas.Generators.RemoveAll(generator => !generator.Nets.IsEmpty());

			_datas.BuildingsPairToAdd.ForEveryKey((pole, buildings) =>
			{
				pole.Net.AddBuildings(buildings);
				pole.AddBuildings(buildings);
				buildings.ForEach(building => building.AddPole(pole));
			});
			_datas.Buildings.RemoveAll(generator => !generator.Nets.IsEmpty());

			_datas.GeneratorsToRemove.ForEach(generator =>
			{
				_datas.GeneratorsPairToRemove.AddUniqueRange(generator,
				                                             generator.NearlyPoles);
			});
			_datas.GeneratorsPairToRemove.ForEveryKey((generator, poles) =>
			{
				generator.RemovePoles(poles);
				poles.ForEach(pole => { pole.RemoveGenerator(generator); });
				var netWasRemoved = false;
				foreach (var pole in poles.Where(pole => !generator.NearlyPoles.Select(_ => _.Net).Distinct()
				                                                   .Contains(pole.Net)))
				{
					if (generator.Nets.Contains(pole.Net))
					{
						pole.Net.RemoveGenerator(generator);
						netWasRemoved = true;
					}
				}

				if (netWasRemoved && generator.Nets.IsEmpty())
				{
					_datas.Generators.AddUnique(generator);
				}
			});

			_datas.BuildingsToRemove.ForEach(building =>
			{
				_datas.BuildingsPairToRemove.AddUniqueRange(building,
				                                            building.NearlyPoles);
			});
			_datas.BuildingsPairToRemove.ForEveryKey((building, poles) =>
			{
				building.RemovePoles(poles);
				poles.ForEach(pole => { pole.RemoveBuilding(building); });
				var netWasRemoved = false;
				foreach (var pole in poles.Where(pole => !building.NearlyPoles.Select(_ => _.Net).Distinct()
				                                                  .Contains(pole.Net)))
				{
					if (building.Nets.Contains(pole.Net))
					{
						pole.Net.RemoveBuilding(building);
						netWasRemoved = true;
					}
				}

				if (netWasRemoved && building.Nets.IsEmpty())
				{
					_datas.Buildings.AddUnique(building);
				}
			});

			foreach (var pole in _datas.PolesToRemove)
			{
				pole.Net?.RemovePole(pole);
			}

			// Добавить метку, что удалено
			_datas.Buildings.RemoveAll(_datas.BuildingsToRemove.Contains);
			_datas.Generators.RemoveAll(_datas.GeneratorsToRemove.Contains);

			_datas.ClearTemp();

			Factory.ReturnItem(this);
			return Factory.GetFactoryItem<WaitingElectricityControllerState>().Initialize(this);
		}

		private ElectricityNet CreateNewNet(IDFactory idFactory)
		{
			var newNet = Factory.GetFactoryItem<ElectricityNet>();
			newNet.Initialize(idFactory.Pop());
			_datas.Nets.Add(newNet.ID, newNet);
			return newNet;
		}

		private ElectricityNet CreateNewNet(IDFactory idFactory, List<IElectricalPoleController> poles)
		{
			var newNet = Factory.GetFactoryItem<ElectricityNet>();
			newNet.Initialize(idFactory.Pop(), poles);
			_datas.Nets.Add(newNet.ID, newNet);
			return newNet;
		}
	}
}