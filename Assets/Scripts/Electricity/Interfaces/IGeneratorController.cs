using System.Collections.Generic;
using Buildings.Interfaces;
using UniRx;

namespace Electricity.Interfaces
{
	public interface IGeneratorController : INetBuildingController
	{
		public IReadOnlyReactiveProperty<(IGeneratorController, float)> ActualPower { get; }
		public IGeneratorModel                                          Model       { get; }

	}
}