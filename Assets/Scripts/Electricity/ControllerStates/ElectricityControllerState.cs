using Factories;

namespace Electricity.ControllerStates
{
	public abstract class ElectricityControllerState
	{
		protected ElectricityControllerDatas _datas;

		public void Initialize(ElectricityControllerDatas datas)
		{
			_datas = datas;
		}

		public ElectricityControllerState Initialize(ElectricityControllerState prevState)
		{
			Initialize(prevState._datas);
			return this;
		}

		public abstract ElectricityControllerState Do(IDFactory idFactory);
	}
}