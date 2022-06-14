using System.Collections.Generic;
using Buildings.Interfaces;
using UnityEngine;

namespace Electricity.Interfaces
{
	public interface IElectricalPoleController : IBuildingController
	{
		public float                                        Wires            { get; }
		public Rect                                         Electricity      { get; }
		public IElectricalPoleModel                         Model            { get; }
		public IReadOnlyList<IElectricalPoleController>     NearlyPoles      { get; }
		public IReadOnlyList<IGeneratorController>          NearlyGenerators { get; }
		public IReadOnlyList<IElectricalBuildingController> NearlyBuildings  { get; }
		public IElectricityNet                              Net              { get; }

		public void AddGenerator(IGeneratorController                   generator);
		public void RemoveGenerator(IGeneratorController                generator);
		public void AddGenerators(List<IGeneratorController>            generators);
		public void RemoveGenerators(List<IGeneratorController>         generators);
		public void AddPole(IElectricalPoleController                   pole);
		public void RemovePole(IElectricalPoleController                pole);
		public void AddPoles(IElectricalPoleController[]                nearlyPoles);
		public void RemovePoles(IEnumerable<IElectricalPoleController>  nearlyPoles);
		public void SetNet(IElectricityNet                              net);
		public void AddBuilding(IElectricalBuildingController           building);
		public void RemoveBuilding(IElectricalBuildingController        building);
		public void AddBuildings(List<IElectricalBuildingController>    buildings);
		public void RemoveBuildings(List<IElectricalBuildingController> buildings);
	}
}