using Zenject;

namespace Game.States
{
	public interface IGameState
	{
		IGameState OnUpdate(GameModel _model, DiContainer _container);
	}
}