using System;
using Electricity.Interfaces;

namespace Electricity
{
	public interface IElectricityController : IDisposable
	{
		void AddGenerator(IGeneratorController            generator);
		void RemoveGenerator(IGeneratorController         generator);
		void AddBuilding(IElectricalBuildingController    building);
		void RemoveBuilding(IElectricalBuildingController building);

		void MergePoles(IElectricalPoleController   newPole, IElectricalPoleController mainPole);

		void AddPole(IElectricalPoleController pole);
		void RemovePole(IElectricalPoleController pole);

		void AddGeneratorToNet(IGeneratorController              generator, IElectricalPoleController pole);
		void RemoveGeneratorFromNet(IGeneratorController         generator, IElectricalPoleController pole);
		void AddBuildingToNet(IElectricalBuildingController      building,  IElectricalPoleController pole);
		void RemoveBuildingFromNet(IElectricalBuildingController building,  IElectricalPoleController pole);
	}
}