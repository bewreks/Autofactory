using System;
using System.Collections.Generic;
using Buildings.Models;
using Factories;
using ModestTree;
using UnityEngine;

namespace Electricity
{
	public class ElectricityController : IDisposable
	{
		private Dictionary<int, ElectricityNet> _nets = new Dictionary<int, ElectricityNet>();

		private List<(Vector3, BaseGeneratorBuildingModel, Rect)> _generators =
			new List<(Vector3, BaseGeneratorBuildingModel, Rect)>();
		
		private List<(Vector3, BaseGeneratorBuildingModel, Rect)> _toRemove =
			new List<(Vector3, BaseGeneratorBuildingModel, Rect)>();

		private IDFactory _idFactory = new IDFactory();

		public void AddToNet(Vector3 position, ElectricPoleBuildingModel poleBuildingModel, int netId)
		{
			if (!_nets.TryGetValue(netId, out var net))
			{
				net = Factory.GetFactoryItem<ElectricityNet>();
				net.Initialize(netId);
				_nets.Add(netId, net);
			}

			net.Add(position, poleBuildingModel);
			
			foreach (var generator in _generators)
			{
				if (AddGenerator(generator.Item1, generator.Item2, generator.Item3))
				{
					_toRemove.Add(generator);
				}
			}
			
			foreach (var generator in _toRemove)
			{
				_generators.Remove(generator);
			}
			_toRemove.Clear();
		}

		public void AddToNet(Vector3 position, ElectricPoleBuildingModel poleBuildingModel)
		{
			AddToNet(position, poleBuildingModel, _idFactory.Pop());
		}

		public void Unite(List<int> nets)
		{
			if (_nets.TryGetValue(nets[0], out var net))
			{
				for (var i = 1; i < nets.Count; i++)
				{
					if (_nets.TryGetValue(nets[0], out var newNet))
					{
						net.Add(newNet);
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

		public void AddGenerator(Vector3 generatorPosition, BaseGeneratorBuildingModel generatorModel)
		{
			var rect = new Rect(new Vector2(generatorPosition.x, generatorPosition.z),
			                    generatorModel.BuildingSize);
			if (!AddGenerator(generatorPosition, generatorModel, rect))
			{
				_generators.Add((generatorPosition, generatorModel, rect));
			}
		}

		public bool AddGenerator(Vector3 generatorPosition, BaseGeneratorBuildingModel generatorModel, Rect rect)
		{
			foreach (var net in _nets.Values)
			{
				if (net.Intersect(rect))
				{
					net.Add(rect, generatorModel);
					return true;
				}
			}

			return false;
		}
	}
}