using Factories;

namespace Electricity.ControllerStates
{
	public class AddPolesElectricityControllerState : ElectricityControllerState
	{
		public override ElectricityControllerState Do(IDFactory idFactory)
		{
			foreach (var pole in _polesToAdd)
			{
				var electricityNet = new ElectricityNet_Old();
				electricityNet.Initialize(idFactory.Pop());
				pole.SetNet(electricityNet);
				_nets.Add(electricityNet.ID, electricityNet);
			}
			_polesToAdd.Clear();

			return NextState();
		}
	}
}