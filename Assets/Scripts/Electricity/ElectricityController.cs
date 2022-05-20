﻿using System;
using System.Collections.Generic;
using System.Linq;
using Buildings.Models;
using Electricity.Controllers;
using Factories;
using UnityEngine;

namespace Electricity
{
	public class ElectricityController : IDisposable
	{
		private Dictionary<int, ElectricityNet> _nets = new Dictionary<int, ElectricityNet>();

		private List<GeneratorController> _generators =
			new List<GeneratorController>();

		private List<GeneratorController> _toRemove =
			new List<GeneratorController>();

		private IDFactory _idFactory = new IDFactory();

		public ElectricityPoleController AddPole(Vector3 position, ElectricPoleBuildingModel poleBuildingModel, int netId)
		{
			if (!_nets.TryGetValue(netId, out var net))
			{
				net = Factory.GetFactoryItem<ElectricityNet>();
				net.Initialize(netId);
				_nets.Add(netId, net);
			}

			var poleController = new ElectricityPoleController(position, poleBuildingModel);
			net.AddPole(poleController);

			foreach (var generator in _generators)
			{
				if (AddGenerator(generator))
				{
					_toRemove.Add(generator);
				}
			}

			foreach (var generator in _toRemove)
			{
				_generators.Remove(generator);
			}

			_toRemove.Clear();

			return poleController;
		}

		public ElectricityPoleController AddPole(Vector3 position, ElectricPoleBuildingModel poleBuildingModel)
		{
			return AddPole(position, poleBuildingModel, _idFactory.Pop());
		}

		public void Unite(List<int> nets)
		{
			if (_nets.TryGetValue(nets[0], out var net))
			{
				for (var i = 1; i < nets.Count; i++)
				{
					if (_nets.TryGetValue(nets[0], out var newNet))
					{
						net.AddNet(newNet);
						_idFactory.Push(newNet.ID);
						_nets.Remove(newNet.ID);
						newNet.Dispose();
						Factory.ReturnItem(newNet);
					}
				}
			}
		}

		public void Dispose()
		{
			foreach (var net in _nets.Values)
			{
				net.Dispose();
			}
		}

		public GeneratorController AddGenerator(Vector3 generatorPosition, BaseGeneratorBuildingModel generatorModel)
		{
			var generator = new GeneratorController(generatorPosition, generatorModel);
			if (!AddGenerator(generator))
			{
				_generators.Add(generator);
			}

			return generator;
		}

		public bool AddGenerator(GeneratorController generator)
		{
			foreach (var net in _nets.Values)
			{
				if (net.IsBuildingInElectricity(generator, out var fsadf))
				{
					net.AddGenerator(generator);
					return true;
				}
			}

			return false;
		}

		public ElectricityNet GetNet(int netId)
		{
			_nets.TryGetValue(netId, out var net);
			return net;
		}

#if UNITY_INCLUDE_TESTS
		public List<GeneratorController> Generators => _generators;
#endif
		public void RemoveGenerator(GeneratorController generator)
		{
			generator.Nets.ToList().ForEach(net =>
			{
				// net.RemoveGenerator(generator);
			});
		}

		public void RemovePole(ElectricityPoleController pole)
		{
			
		}

		public void ReturnGenerators(IEnumerable<GeneratorController> generators)
		{
			_generators.AddRange(generators);
		}
	}
}