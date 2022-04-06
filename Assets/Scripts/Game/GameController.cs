using System;
using Factories;
using Game.States;
using UniRx;
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
		public void Construct()
		{
			_state = Factory.GetFactoryItem<NormalGameState>(_diContainer);

			_model = _diContainer.Resolve<IGameModel>() as GameModel;
			
			Observable.EveryUpdate().Subscribe(l =>
			{
				_state = _state.OnUpdate(_model, _diContainer);
			}).AddTo(_disposables);
		}

		public void Dispose()
		{
			_disposables?.Dispose();
		}
	}
}