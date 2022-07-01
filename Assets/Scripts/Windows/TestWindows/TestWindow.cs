using UnityEngine;

namespace Windows.TestWindows
{
	[CreateAssetMenu(fileName = "TestWindow", menuName = "Models/Windows/TestWindow")]
	public class TestWindow : Window
	{
		protected override IWindowController CreateWindowController()
		{
			return new TestWindowController(_view, Data);
		}
	}

	internal class TestWindowController : WindowController
	{
		public TestWindowController(WindowView view, Window.WindowData data) : base(view, data) { }
		public override void PrepareView()
		{
			
		}
	}
}