using Factories;
using Installers;
using Zenject;

namespace Game.States
{
	public class WindowGameState : IGameState
	{
		[Inject] private DiContainer    _diContainer;
		[Inject] private WindowsManager _windowsManager;
		
		public IGameState OnUpdate(GameModel model, DiContainer container)
		{
			if (_windowsManager.IsOpened())
			{
				return this;
			}
			else
			{
				Factory.ReturnItem(this);
				return Factory.GetFactoryItem<NormalGameState>(_diContainer);
			}
		}

		public void OnFixedUpdate(GameModel model) { }
	}
}