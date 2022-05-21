using System;
using Buildings.Models;

namespace Buildings.Views
{
	public class BaseBuildingView : BuildingView
	{
		protected override Type ModelType => typeof(BuildingModel);
	}
}