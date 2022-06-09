using System.Collections.Generic;

namespace Electricity.Interfaces
{
	public interface INetBuildingController : IBuildingController
	{
		public List<IElectricalPoleController> NearlyPoles { get; }
		public IReadOnlyList<IElectricityNet>  Nets        { get; }

		public void AddNet(IElectricityNet                      net);
		public void RemoveNet(IElectricityNet                   net);
		public void AddPole(IElectricalPoleController           pole);
		public void RemovePole(IElectricalPoleController        pole);
		public void RemovePoles(List<IElectricalPoleController> poles);
	}
}