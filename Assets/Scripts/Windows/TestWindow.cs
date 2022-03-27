using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

namespace Windows
{
	public class TestWindow : Window
	{
		[SerializeField] private Button      closeButton;
		[SerializeField] private CanvasGroup canvasGroup;
		
		protected override void Opening()
		{
			canvasGroup.alpha = 0;
			canvasGroup.DOFade(1, 0.25f).OnComplete(Opened);
			closeButton.onClick.AddListener(Close);
		}

		protected override void Closing()
		{
			canvasGroup.DOFade(0, 0.25f).OnComplete(Closed);
			closeButton.onClick.RemoveAllListeners();
		}
	}
}