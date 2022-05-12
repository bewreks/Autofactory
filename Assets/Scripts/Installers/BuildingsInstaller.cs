using System;
using System.Collections.Generic;
using System.Linq;
using Buildings.Models;
using Inventories;
using UnityEngine;
using Zenject;

namespace Installers
{
	[CreateAssetMenu(fileName = "BuildingsInstaller", menuName = "Installers/BuildingsInstaller")]
	public class BuildingsInstaller : ScriptableObjectInstaller<BuildingsInstaller>
	{
		[SerializeField] private BuildingsModelsSettings settings;

		public override void InstallBindings()
		{
			settings.Prepare();
			Container.Bind<BuildingsModelsSettings>().FromInstance(settings).AsSingle();
		}
	}

	[Serializable]
	public class BuildingsModelsSettings
	{
		[SerializeField] private List<BuildingModel> _models = new List<BuildingModel>();

		private Dictionary<InventoryObjectsTypesEnum, BuildingModel> _buildingsMap =
			new Dictionary<InventoryObjectsTypesEnum, BuildingModel>();

		public bool Prepare()
		{
			try
			{
				_buildingsMap = _models.ToDictionary(model => model.Type, model => model);
			}
			catch (Exception)
			{
				return false;
			}

			return true;
		}
	}
}