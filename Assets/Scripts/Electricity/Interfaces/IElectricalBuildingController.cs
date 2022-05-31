using System.Collections.Generic;
using Buildings.Interfaces;

namespace Electricity.Interfaces
{
	public interface IElectricalBuildingController : IBuildingController
	{
		public List<ElectricityNet>            Nets        { get; }
		public List<IElectricalPoleController> NearlyPoles { get; }
		public IElectricalBuildingModel        Model       { get; }

		public void AddNet(ElectricityNet                       net);
		public void RemoveNet(ElectricityNet                    net);
		public void AddPole(IElectricalPoleController           pole);
		public void RemovePole(IElectricalPoleController        pole);
		public void RemovePoles(List<IElectricalPoleController> poles);
	}
}