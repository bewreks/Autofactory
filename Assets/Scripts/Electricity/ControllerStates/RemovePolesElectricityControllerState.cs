using Factories;

namespace Electricity.ControllerStates
{
	public class RemovePolesElectricityControllerState : ElectricityControllerState
	{
		public override ElectricityControllerState Do(IDFactory idFactory)
		{
			foreach (var pole in _polesToRemove)
			{
				if (pole.Net != null)
				{
					pole.Net.RemovePole(pole);
				}
			}

			return NextState();
		}
	}
}