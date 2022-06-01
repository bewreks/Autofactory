using System;
using System.Collections.Generic;

namespace Electricity.Interfaces
{
	public interface IElectricityNet : IDisposable
	{
		public int                                 ID         { get; }
		public float                               Power      { get; }		
		public List<IGeneratorController>          Generators { get; }
		public List<IElectricalPoleController>     Poles      { get; }
		public List<IElectricalBuildingController> Buildings  { get; }


		public void Initialize(int netId);
		public void Initialize(int netId, List<IElectricalPoleController> poles);

		public void Unite(IElectricityNet net);

		public void AddPole(IElectricalPoleController                   pole);
		public void RemovePole(IElectricalPoleController                pole);
		public void AddPoles(List<IElectricalPoleController>            poles);
		public void RemovePoles(List<IElectricalPoleController>         poles);
		public void AddGenerator(IGeneratorController                   generator);
		public void RemoveGenerator(IGeneratorController                generator);
		public void AddGenerators(List<IGeneratorController>            generators);
		public void RemoveGenerators(List<IGeneratorController>         generators);
		public void AddBuilding(IElectricalBuildingController           building);
		public void RemoveBuilding(IElectricalBuildingController        building);
		public void AddBuildings(List<IElectricalBuildingController>    buildings);
		public void RemoveBuildings(List<IElectricalBuildingController> buildings);

		public void StopPowerUpdates();
		public void StartPowerUpdates();
	}
}