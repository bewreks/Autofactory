using System.Collections.Generic;
using Electricity.Controllers;
using Factories;

namespace Electricity
{
	public class ElectricityController : IElectricityController
	{
		private List<ElectricityPoleController> _polesNotInNet       = new List<ElectricityPoleController>();
		private List<GeneratorController>       _generatorsNotInNets = new List<GeneratorController>();

		private List<ElectricityConsumptionBuildingController> _consumptionsNotInNet =
			new List<ElectricityConsumptionBuildingController>();

		private Dictionary<int, ElectricityNet_Old> _nets = new Dictionary<int, ElectricityNet_Old>();

		private IDFactory _idFactory = new IDFactory();

		public void AddGenerator(GeneratorController generator)
		{
			throw new System.NotImplementedException();
		}

		public void RemoveGenerator(GeneratorController generator)
		{
			throw new System.NotImplementedException();
		}

		public void AddBuilding(ElectricityConsumptionBuildingController building, ElectricityPoleController pole)
		{
			throw new System.NotImplementedException();
		}

		public void RemoveBuilding(ElectricityConsumptionBuildingController building, ElectricityPoleController pole)
		{
			throw new System.NotImplementedException();
		}

		public void MergePoles(ElectricityPoleController newPole, ElectricityPoleController mainPole)
		{
			throw new System.NotImplementedException();
		}

		public void UnmergePoles(ElectricityPoleController newPole, ElectricityPoleController mainPole)
		{
			throw new System.NotImplementedException();
		}

		public void AddPole(ElectricityPoleController pole)
		{
			var net = Factory.GetFactoryItem<ElectricityNet>();
			net.Initialize(_idFactory.Pop());
			net.AddPole(pole);
		}

		public void AddGeneratorToNet(GeneratorController generator, ElectricityPoleController pole)
		{
			throw new System.NotImplementedException();
		}

		public void RemoveGeneratorFromNet(GeneratorController generator, ElectricityPoleController pole)
		{
			throw new System.NotImplementedException();
		}
		
		public void Dispose()
		{
			throw new System.NotImplementedException();
		}
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