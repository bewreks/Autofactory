using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

namespace Windows.TestWindows
{
	public class TestWindow : Window
	{
		[SerializeField] private Button      closeButton;
		[SerializeField] private CanvasGroup canvasGroup;
		[SerializeField] private float       _duration = 0.25f;

		public float Duration => _duration;

		protected override void Opening()
		{
			canvasGroup.alpha = 0;
			canvasGroup.DOFade(1, _duration).OnComplete(Opened);
			closeButton.onClick.AddListener(Close);
		}

		protected override void Closing()
		{
			canvasGroup.DOFade(0, _duration).OnComplete(Closed);
			closeButton.onClick.RemoveAllListeners();
		}
	}
}