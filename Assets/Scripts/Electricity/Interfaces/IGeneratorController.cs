using System.Collections.Generic;
using Buildings.Interfaces;
using UniRx;

namespace Electricity.Interfaces
{
	public interface IGeneratorController : IBuildingController
	{
		public IReadOnlyReactiveProperty<float> ActualPower { get; }
		public IGeneratorModel                  Model       { get; }
		public List<ElectricityNet>             Nets        { get; }
		public List<IElectricalPoleController>   NearlyPoles { get; }

		public void AddNet(ElectricityNet net);
		public void RemoveNet(ElectricityNet net);
		public void AddPole(IElectricalPoleController pole);
		public void RemovePole(IElectricalPoleController pole);
		public void RemovePoles(List<IElectricalPoleController> poles);
	}
}