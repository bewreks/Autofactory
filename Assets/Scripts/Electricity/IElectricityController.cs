using System;
using Electricity.Controllers;

namespace Electricity
{
	public interface IElectricityController : IDisposable
	{
		void AddGenerator(GeneratorController    generator);
		void RemoveGenerator(GeneratorController generator);

		void AddBuilding(ElectricityConsumptionBuildingController    building, ElectricityPoleController pole);
		void RemoveBuilding(ElectricityConsumptionBuildingController building, ElectricityPoleController pole);

		void MergePoles(ElectricityPoleController   newPole, ElectricityPoleController mainPole);
		void UnmergePoles(ElectricityPoleController newPole, ElectricityPoleController mainPole);

		void AddPole(ElectricityPoleController pole);

		void AddGeneratorToNet(GeneratorController      generator, ElectricityPoleController pole);
		void RemoveGeneratorFromNet(GeneratorController generator, ElectricityPoleController pole);
	}
}