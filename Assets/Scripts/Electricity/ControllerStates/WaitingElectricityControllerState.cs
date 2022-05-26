using Factories;

namespace Electricity.ControllerStates
{
	public class WaitingElectricityControllerState : ElectricityControllerState
	{
		public override bool Waiting => true;

		public override ElectricityControllerState Do(IDFactory idFactory)
		{
			return this;
		}
	}
}