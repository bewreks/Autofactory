using System;
using Factories;
using Game.States;
using Inventories;
using NUnit.Framework;
using Players;
using UniRx;
using UnityEngine;
using Zenject;

namespace Game
{
	public class GameController : IDisposable
	{
		[Inject] private DiContainer _diContainer;

		private GameModel           _model;
		private IGameState          _state;
		private CompositeDisposable _disposables = new CompositeDisposable();

		[Inject]
		public void Construct(GameSettings gameSettings)
		{
			_model             = new GameModel();
			_model.PlayerModel = _diContainer.InstantiatePrefabForComponent<PlayerModel>(gameSettings.PlayerPrefab);
			_disposables.Add(_model);
			_diContainer.Inject(_model.PlayerModel.Inventory);
			_diContainer.BindInterfacesTo<GameModel>().FromInstance(_model).AsSingle();

			_state = Factory.GetFactoryItem<NormalGameState>(_diContainer);

			Observable.EveryUpdate()
			          .Subscribe(l => { _state = _state.OnUpdate(_model, _diContainer); })
			          .AddTo(_disposables);
			
			Observable.EveryFixedUpdate()
			          .Subscribe(l => { _state.OnFixedUpdate(_model); })
			          .AddTo(_disposables);
		}

		public void Dispose()
		{
			_disposables?.Dispose();
		}

		public void RotatePlayerTo(Vector3 rotateTo)
		{
			rotateTo.y = _model.PlayerModel.PlayerViewModel.transform.position.y;
			_model.PlayerModel.PlayerViewModel.transform.LookAt(rotateTo);
		}
	}
}