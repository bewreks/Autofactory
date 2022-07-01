using System;
using UnityEngine;

namespace Windows
{
	public abstract class WindowView : MonoBehaviour
	{
		public event Action OnClose;

		public abstract void Opening(float duration);
		public abstract void Closing(float duration);
		public abstract void Hiding(float  duration);

		public virtual void BeforeOpen()  { }
		public virtual void AfterOpen()   { }
		public virtual void BeforeClose() { }
		public virtual void AfterClose()  { }

		protected void CastOnClose()
		{
			OnClose?.Invoke();
		}
	}
}