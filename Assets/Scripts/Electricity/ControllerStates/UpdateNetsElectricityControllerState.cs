using System.Collections.Generic;
using System.Linq;
using Electricity.Controllers;
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
			// 1. Добавить столбы
			// 2.0 Удалить столбы
			// 2.1 Сохранить столбы-соседи для раделения
			// 3. Разделить сети
			// 4. Объединить столбы/сети
			// 5. Добавить генераторы
			// 6. Удалить генераторы
			// 7. Добавить дома
			// 8. Удалить дома
			
			foreach (var pole in _datas.PolesToAdd)
			{
				var newNet = CreateNewNet(idFactory);
				newNet.AddPole(pole);
			}

			var toRemove = new List<List<ElectricityPoleController>>();
			foreach (var pole in _datas.PolesToRemove)
			{
				toRemove.Add(pole.NearlyPoles);
				pole.Net.RemovePole(pole);
			}
			
			foreach (var neighbours in toRemove)
			{
				var list = new List<List<ElectricityPoleController>>();
				var i    = -1;
				foreach (var nearlyPole in neighbours.Where(nearlyPole => i < 0 || 
				                                                          !list[i].Contains(nearlyPole)))
				{
					list.Add(new List<ElectricityPoleController>());
					list[++i].Add(nearlyPole);
					nearlyPole.NearlyPoles.UnitePoles(list[i]);
				}

				if (list.Count <= 1)
				{
					continue;
				}

				for (i = 1; i < list.Count; i++)
				{
					CreateNewNet(idFactory, list[i]);
				}
			}

			foreach (var pair in _datas.ToMerge)
			{
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
			});
			_datas.Generators.RemoveAll(generator => !generator.Nets.IsEmpty());
			
			_datas.BuildingsPairToAdd.ForEveryKey((pole, buildings) =>
			{
				pole.Net.AddBuildings(buildings);
				pole.AddBuildings(buildings);
			});
			_datas.Buildings.RemoveAll(generator => !generator.Nets.IsEmpty());
			
			_datas.GeneratorsPairToRemove.ForEveryKey((generator, poles) =>
			{
				generator.RemovePoles(poles);
				foreach (var pole in poles.Where(pole => !generator.NearlyPoles.Select(_ => _.Net).Distinct().Contains(pole.Net)))
				{
					generator.Nets.Remove(pole.Net);
				}
				if (generator.Nets.IsEmpty())
				{
					_datas.Generators.AddUnique(generator);
				}
			});
			
			_datas.BuildingsPairToRemove.ForEveryKey((building, poles) =>
			{
				building.RemovePoles(poles);
				if (building.Nets.IsEmpty())
				{
					_datas.Buildings.AddUnique(building);
				}
			});

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

		private ElectricityNet CreateNewNet(IDFactory idFactory, List<ElectricityPoleController> poles)
		{
			var newNet = Factory.GetFactoryItem<ElectricityNet>();
			newNet.Initialize(idFactory.Pop(), poles);
			_datas.Nets.Add(newNet.ID, newNet);
			return newNet;
		}
	}
}