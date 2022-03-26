using System;
using System.Collections.Generic;
using System.Linq;
using Inventory;
using UnityEngine;
using UnityEngine.Assertions;
using Zenject;

[CreateAssetMenu(fileName = "InventoryPacksModelsManager", menuName = "Models/Inventory/InventoryPacksModelsManager")]
public class InventoryPacksModelsManagerSO : ScriptableObjectInstaller<InventoryPacksModelsManagerSO>
{
    [SerializeField] private InventoryPacksModelsManager _manager;
    
    public override void InstallBindings()
    {
        Container.Bind<InventoryPacksModelsManager>().FromInstance(_manager).AsSingle();
    }
}

[Serializable]
public class InventoryPacksModelsManager
{
    [SerializeField] private List<InventoryPackModel> _models = new List<InventoryPackModel>();

    public InventoryPackModel GetModel(InventoryTypesEnum type)
    {
        var packModel = _models.FirstOrDefault(model => model.Type == type);
        Assert.IsNotNull(packModel, $"Model of type {type} not found");
        return packModel;
    }
}