﻿using Factories;
using Installers;
using Instantiate;
using Players;
using Players.Interfaces;
using UnityEngine;
using Zenject;

namespace Game.States
{
	public class SelectedItemGameState : IGameState
	{
		[Inject] private InstantiateManager    _instantiateManager;
		[Inject] private DiContainer           _diContainer;
		[Inject] private IPlayerInputController _playerInputController;

		public IGameState OnUpdate(GameModel model, DiContainer container)
		{
			if (_playerInputController.UsingAfterActions.UsingCancel.WasPressedThisFrame())
			{
				Object.Destroy(model.InstantiablePack.gameObject);

				model.SelectedPack     = null;
				model.InstantiablePack = null;
				Factory.ReturnItem(this);
				_playerInputController.UsingAfterActions.Disable();
				_playerInputController.UsingBeforeActions.Enable();
				return Factory.GetFactoryItem<NormalGameState>(_diContainer);
			}

			if (_playerInputController.UsingAfterActions.UsingSubmit.WasPressedThisFrame() && !model.InstantiablePack.Triggered)
			{
				_instantiateManager.InstantiateFinal();

				model.SelectedPack     = null;
				model.InstantiablePack = null;
				Factory.ReturnItem(this);
				_playerInputController.UsingAfterActions.Disable();
				_playerInputController.UsingBeforeActions.Enable();
				return Factory.GetFactoryItem<NormalGameState>(_diContainer);
			}

			return this;
		}

		public void OnFixedUpdate(GameModel model)
		{
			_instantiateManager.UpdatePreviewPosition(model.MousePosition);
		}
	}
}