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

		public bool Prepare()
		{
			try
			{
				_craftingMap = _craftingModels.ToDictionary(model => model.CraftingResult.model.Type, model => model);
			}
			catch (Exception)
			{
				return false;
			}

			return true;
		}

		public CraftingModel GetModel(InventoryObjectsTypesEnum type)
		{
			if (_craftingMap == null) return null;

			_craftingMap.TryGetValue(type, out var craftingModel);
			return craftingModel;
		}

		public Dictionary<InventoryObjectsTypesEnum, CraftingModel>.ValueCollection GetModels => _craftingMap.Values;
		public List<CraftingModel> Models => _craftingModels;
	}
}