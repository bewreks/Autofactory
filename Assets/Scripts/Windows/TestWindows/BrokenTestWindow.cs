using UnityEngine;

namespace Windows.TestWindows
{
	[CreateAssetMenu(fileName = "BrokenTestWindow", menuName = "Models/Windows/BrokenTestWindow")]
	public class BrokenTestWindow : Window
	{
		protected override IWindowController CreateWindowController()
		{
			return new BrokenTestWindowController(_view, Data);
		}
	}

	internal class BrokenTestWindowController : WindowController
	{
		public BrokenTestWindowController(WindowView view, Window.WindowData data) : base(view, data) { }
		public override void PrepareView()
		{
			throw new System.NotImplementedException();
		}
	}
}