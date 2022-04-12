using Factories;
using Instantiate;
using Inventory;
using UnityEngine;
using Zenject;

namespace Game.States
{
	public class NormalGameState : IGameState
	{
		[Inject] private InstantiateManager _instantiateManager;
		[Inject] private DiContainer        _diContainer;
		
		public IGameState OnUpdate(GameModel _model, DiContainer _container)
		{
			if (Input.GetKeyUp(KeyCode.Alpha1))
			{
				var inventory = _container.Resolve<IInventory>();
				_model.SelectedPack     = inventory.GetPacks(InventoryTypesEnum.TEST_OBJECT);
				_model.InstantiablePack = _instantiateManager.InstantiatePreview();
				Factory.ReturnItem(this);
				return Factory.GetFactoryItem<SelectedItemGameState>(_diContainer);
			}

			return this;
		}
	}
}