using Factories;

namespace Electricity.ControllerStates
{
	public class AddBuildingsElectricityControllerState : ElectricityControllerState
	{
		public override ElectricityControllerState Do(IDFactory idFactory)
		{
			throw new System.NotImplementedException();
			Factory.ReturnItem(this);
			return Factory.GetFactoryItem<WaitingElectricityControllerState>().Initialize(this);
		}
	}
}