namespace Buildings.Interfaces
{
	public interface IElectricalPoleModel : IBuildingModel
	{
		public float WireRadius      { get; }
		public float ElectricitySize { get; }
	}
}