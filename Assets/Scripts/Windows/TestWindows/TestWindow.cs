using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

namespace Windows.TestWindows
{
	public class TestWindow : WindowView
	{
		[SerializeField] private Button      closeButton;
		[SerializeField] private CanvasGroup canvasGroup;

		public override void Opening()
		{
			canvasGroup.alpha = 0;
			canvasGroup.DOFade(1, _duration).OnComplete(CastOnOpened);
			closeButton.onClick.AddListener(CastOnClose);
		}

		public override void Closing()
		{
			canvasGroup.DOFade(0, _duration).OnComplete(CastOnClosed);
			closeButton.onClick.RemoveAllListeners();
		}

		public override void Hiding()
		{
			canvasGroup.DOFade(0, _duration).OnComplete(CastOnHided);
			closeButton.onClick.RemoveAllListeners();
		}
	}
}