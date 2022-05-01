using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Assertions;
using Zenject;

namespace Inventories
{
    [CreateAssetMenu(fileName = "InventoryPacksModelsManager", menuName = "Models/Inventory/InventoryPacksModelsManager")]
    public class InventoryPacksInstaller : ScriptableObjectInstaller<InventoryPacksInstaller>
    {
        [SerializeField] private InventoryPacksModelsSettings settings;
    
        public override void InstallBindings()
        {
            Container.Bind<InventoryPacksModelsSettings>().FromInstance(settings).AsSingle();
        }
    }

    [Serializable]
    public class InventoryPacksModelsSettings
    {
        [SerializeField] private List<InventoryPackModel> _models = new List<InventoryPackModel>();

        public InventoryPackModel GetModel(InventoryTypesEnum type)
        {
            var packModel = _models.FirstOrDefault(model => model.Type == type);
            Assert.IsNotNull(packModel, $"Model of type {type} not found");
            return packModel;
        }

#if UNITY_INCLUDE_TESTS
        public List<InventoryPackModel> Models => _models;
#endif
    }
}