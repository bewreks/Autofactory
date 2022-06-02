using System.Collections.Generic;
using Buildings.Interfaces;
using UniRx;

namespace Electricity.Interfaces
{
	public interface IGeneratorController : IBuildingController
	{
		public IReadOnlyReactiveProperty<(IGeneratorController, float)> ActualPower { get; }
		public IGeneratorModel                                          Model       { get; }
		public List<IElectricityNet>                                    Nets        { get; }
		public List<IElectricalPoleController>                          NearlyPoles { get; }

		public void AddNet(IElectricityNet                      net);
		public void RemoveNet(IElectricityNet                   net);
		public void AddPole(IElectricalPoleController           pole);
		public void RemovePole(IElectricalPoleController        pole);
		public void RemovePoles(List<IElectricalPoleController> poles);
	}
}