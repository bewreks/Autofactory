using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

namespace Windows.TestWindows
{
	public class TestWindowView : WindowView
	{
		[SerializeField] private Button      closeButton;
		[SerializeField] private CanvasGroup canvasGroup;


		public override void BeforeOpen()
		{
			canvasGroup.alpha = 0;
		}
		public override void Opening(float duration)
		{
			canvasGroup.DOFade(1, duration);
		}

		public override void AfterOpen()
		{
			closeButton.onClick.AddListener(CastOnClose);
		}

		public override void BeforeClose()
		{
			closeButton.onClick.RemoveAllListeners();
		}

		public override void Closing(float duration)
		{
			canvasGroup.DOFade(0, duration);
		}

		public override void Hiding(float duration)
		{
			canvasGroup.DOFade(0, duration);
		}
	}
}