using System;
using Electricity.Controllers;

namespace Electricity
{
	public interface IElectricityController : IDisposable
	{
		void AddGenerator(GeneratorController            generator);
		void RemoveGenerator(GeneratorController         generator);
		void AddBuilding(ElectricalBuildingController    building);
		void RemoveBuilding(ElectricalBuildingController building);

		void MergePoles(ElectricityPoleController   newPole, ElectricityPoleController mainPole);

		void AddPole(ElectricityPoleController pole);
		void RemovePole(ElectricityPoleController pole);

		void AddGeneratorToNet(GeneratorController              generator, ElectricityPoleController pole);
		void RemoveGeneratorFromNet(GeneratorController         generator, ElectricityPoleController pole);
		void AddBuildingToNet(ElectricalBuildingController      building,  ElectricityPoleController pole);
		void RemoveBuildingFromNet(ElectricalBuildingController building,  ElectricityPoleController pole);
	}
}