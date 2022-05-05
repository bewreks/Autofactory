﻿using System;
using Installers;
using UniRx;
using UnityEngine;

namespace Windows
{
	public abstract class Window : MonoBehaviour
	{
		public event Action OnClose;

		private readonly ReactiveProperty<WindowStateEnum>
			_state = new ReactiveProperty<WindowStateEnum>(WindowStateEnum.CLOSED);

		public void Open()
		{
			_state.SetValueAndForceNotify(WindowStateEnum.OPENING);
			Opening();
		}

		public void Close()
		{
			_state.SetValueAndForceNotify(WindowStateEnum.CLOSING);
			Closing();
		}

		protected void Opened()
		{
			_state.SetValueAndForceNotify(WindowStateEnum.OPENED);
		}

		protected void Closed()
		{
			_state.SetValueAndForceNotify(WindowStateEnum.CLOSED);
			OnClose?.Invoke();
			Destroy(gameObject);
		}

		protected abstract void Opening();
		protected abstract void Closing();

#if UNITY_INCLUDE_TESTS
		public ReactiveProperty<WindowStateEnum> State => _state;
#endif
	}
}