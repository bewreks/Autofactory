using Zenject;

namespace Game.States
{
	public interface IGameState
	{
		IGameState OnUpdate(GameModel      model, DiContainer container);
		void       OnFixedUpdate(GameModel model);
	}
}