using System;
using Buildings.Models;

namespace Buildings.Views
{
	public class BaseBuildingView : BuildingView
	{
		protected override Type ModelType     => typeof(BuildingModel);
		protected override int  BuildingLayer => gameSettings.DefaultLayer;
		protected override void OnFinalInstantiate()
		{
			
		}
	}
}