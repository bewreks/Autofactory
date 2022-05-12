using System;
using System.Collections.Generic;
using System.Linq;
using Inventories;
using NUnit.Framework;
using UnityEngine;
using Zenject;

namespace Installers
{
	[CreateAssetMenu(fileName = "InventoryPacksModelsManager", menuName = "Installers/InventoryInstaller")]
	public class InventoryPacksInstaller : ScriptableObjectInstaller<InventoryPacksInstaller>
	{
		[SerializeField] private InventoryPacksModelsSettings settings;

		public override void InstallBindings()
		{
			settings.Prepare();
			Container.Bind<InventoryPacksModelsSettings>().FromInstance(settings).AsSingle();
		}
	}

	[Serializable]
	public class InventoryPacksModelsSettings
	{
		[SerializeField] private List<InventoryPackModel> _models = new List<InventoryPackModel>();

		[SerializeField]
		private List<InventoryTypesModel> _inventories = new List<InventoryTypesModel>();

		private Dictionary<InventoryTypesEnum, InventoryTypesModel> _inventoriesMap =
			new Dictionary<InventoryTypesEnum, InventoryTypesModel>();

		public InventoryPackModel GetModel(InventoryObjectsTypesEnum type)
		{
			var packModel = _models.FirstOrDefault(model => model.Type == type);
			if (packModel == null)
				throw new Exception($"Model of type {type} not found");
			return packModel;
		}

		public InventoryTypesModel GetInventoryModel(InventoryTypesEnum type)
		{
			if (_inventoriesMap.TryGetValue(type, out var model))
			{
				return model;
			}
			Assert.Fail($"Model of type {type} not found");
			return null;
		}

#if UNITY_INCLUDE_TESTS
		public List<InventoryPackModel>  Models      => _models;
		public List<InventoryTypesModel> Inventories => _inventories;
#endif
		public bool Prepare()
		{
			try
			{
				_inventoriesMap =
					_inventories.ToDictionary(inventory => inventory.InventoryType, inventory => inventory);
			}
			catch (Exception)
			{
				return false;
			}

			return true;
		}
	}
}