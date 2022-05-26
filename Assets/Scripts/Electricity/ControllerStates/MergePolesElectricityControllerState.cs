using System.Linq;
using Factories;

namespace Electricity.ControllerStates
{
	public class MergePolesElectricityControllerState : ElectricityControllerState
	{
		public override ElectricityControllerState Do(IDFactory idFactory)
		{
			foreach (var pair in _toMerge.Values.SelectMany(pairs => pairs))
			{
				ElectricityNet_Old net;
					
				switch (pair.Nets.Length)
				{
					case 0:
						net = Factory.GetFactoryItem<ElectricityNet_Old>();
						net.Initialize(idFactory.Pop());
						net.AddPole(pair.From);
						net.AddPole(pair.To);
						break;
					case 1:
						net = pair.Nets[0];
						net.AddPole(pair.From);
						net.AddPole(pair.To);
						break;
					case 2:
						pair.From.Net.AddNet(pair.To.Net);
						break;
				}
			}
			
			_toMerge.Clear();

			return NextState();
		}
	}
}