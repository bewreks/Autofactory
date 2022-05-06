using System;
using System.Collections.Generic;
using System.Linq;
using Crafting;
using Inventories;
using UnityEngine;
using Zenject;

namespace Installers
{
	[CreateAssetMenu(fileName = "CraftSettingsInstaller", menuName = "Installers/CraftSettingsInstaller")]
	public class CraftSettingsInstaller : ScriptableObjectInstaller<CraftSettingsInstaller>
	{
		[SerializeField] private CraftSettings settings;

		public override void InstallBindings()
		{
			settings.Prepare();
			Container.Bind<CraftSettings>().FromInstance(settings).AsSingle();
		}
	}
	
	[Serializable]
	public class CraftSettings
	{
		[SerializeField] private List<CraftingModel> _craftingModels = new List<CraftingModel>();

		private Dictionary<InventoryObjectsTypesEnum, CraftingModel> _craftingMap =
			new Dictionary<InventoryObjectsTypesEnum, CraftingModel>();

		public void Prepare()
		{
			_craftingMap = _craftingModels.ToDictionary(model => model.CraftingResult.model.Type, model => model);
		}
		
		public CraftingModel GetModel(InventoryObjectsTypesEnum type)
		{
			if (_craftingMap == null) return null;
			
			_craftingMap.TryGetValue(type, out var craftingModel);
			return craftingModel;
		}
	}
}