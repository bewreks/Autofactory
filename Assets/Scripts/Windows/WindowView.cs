using System;
using UnityEngine;

namespace Windows
{
	public abstract class WindowView : MonoBehaviour
	{
		[SerializeField] protected float _duration = 0.25f;
		
		public event Action OnClose;
		public event Action OnClosed;
		public event Action OnHided;
		public event Action OnOpened;
		
		public float Duration => _duration;

		public abstract void Opening();
		public abstract void Closing();
		public abstract void Hiding();

		protected void CastOnClose()
		{
			OnClose?.Invoke();
		}

		protected void CastOnClosed()
		{
			OnClosed?.Invoke();
		}

		protected void CastOnHided()
		{
			OnHided?.Invoke();
		}

		protected void CastOnOpened()
		{
			OnOpened?.Invoke();
		}
	}
}