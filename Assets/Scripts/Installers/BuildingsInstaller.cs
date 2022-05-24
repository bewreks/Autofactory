using System;
using System.Collections.Generic;
using System.Linq;
using Buildings.Colliders;
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
		[SerializeField] private ColliderView        _squareCollider;
		[SerializeField] private ColliderView        _wiresCollider;

		private Dictionary<InventoryObjectsTypesEnum, BuildingModel> _buildingsMap =
			new Dictionary<InventoryObjectsTypesEnum, BuildingModel>();

		public ColliderView SquareCollider => _squareCollider;
		public ColliderView WiresCollider  => _wiresCollider;

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

#if UNITY_INCLUDE_TESTS
		public List<BuildingModel> Models => _models;
#endif
	}
}