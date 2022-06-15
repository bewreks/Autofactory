using System;
using DG.Tweening;
using Factories;
using Game.States;
using Installers;
using Players;
using Players.Interfaces;
using UniRx;
using UnityEngine;
using Zenject;

namespace Game
{
	public class GameController : IDisposable
	{
		[Inject] private DiContainer           _diContainer;
		[Inject] private GameSettings          _gameSettings;
		[Inject] private IPlayerInputController _playerInputController;

		private GameModel           _model;
		private IGameState          _state;
		private CompositeDisposable _disposables = new CompositeDisposable();

		[Inject]
		public void Construct(GameSettings gameSettings)
		{
			_model = new GameModel();
			_diContainer.BindInterfacesTo<GameModel>().FromInstance(_model).AsSingle();
			_model.PlayerModel = _diContainer.InstantiatePrefabForComponent<PlayerModel>(gameSettings.PlayerPrefab);
			_disposables.Add(_model);
			_diContainer.Inject(_model.PlayerModel.Inventory);

			_state = Factory.GetFactoryItem<NormalGameState>(_diContainer);

			Observable.EveryUpdate()
			          .Subscribe(l =>
			          {
				          if (!(Camera.main is null))
				          {
					          if (PlayerInputHelper.GetWorldMousePosition(_gameSettings.GroundMask, Camera.main,
					                                                      _playerInputController, out var mousePosition))
					          {
						          _model.MousePosition = mousePosition;
					          }

					          var deltaTime    = Time.deltaTime;
					          var currentInput = PlayerInputHelper.GetPlayerInput(Camera.main, deltaTime, _playerInputController);
					          _model.MoveDelta += currentInput;
				          }
				          _state = _state.OnUpdate(_model, _diContainer);
			          })
			          .AddTo(_disposables);

			Observable.EveryFixedUpdate()
			          .Subscribe(l =>
			          {
				          _state.OnFixedUpdate(_model);
				          RotatePlayerTo();
				          MovePlayer();
				          _model.MoveDelta = Vector3.zero;
			          })
			          .AddTo(_disposables);
		}

		public void Dispose()
		{
			_disposables?.Dispose();
		}

		private void RotatePlayerTo()
		{
			var rotateTo =_model.MousePosition; 
			rotateTo.y = _model.PlayerModel.PlayerViewModel.transform.position.y;
			_model.PlayerModel.PlayerViewModel.transform.DOLookAt(rotateTo, _gameSettings.RotationSpeed);
		}

		public void MovePlayer()
		{
			_model.PlayerModel.Rigidbody.velocity = _model.MoveDelta * (_gameSettings.MoveSpeed * 50);
		}
	}
}