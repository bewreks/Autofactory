using System.Collections.Generic;
using Buildings.Interfaces;

namespace Electricity.Interfaces
{
	public interface IElectricalBuildingController : INetBuildingController
	{
		public IElectricalBuildingModel Model { get; }
	}
}