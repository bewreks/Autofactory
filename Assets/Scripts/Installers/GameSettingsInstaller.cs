using System;
using UnityEngine;
using Zenject;

namespace Game
{
	[CreateAssetMenu(fileName = "GameSettingsInstaller", menuName = "Installers/GameSettingsInstaller")]
	public class GameSettingsInstaller : ScriptableObjectInstaller<GameSettingsInstaller>
	{
		[SerializeField] private PlayersSetting settings;
    
		public override void InstallBindings()
		{
			Container.Bind<PlayersSetting>().FromInstance(settings).AsSingle();
		}
	}
	
	[Serializable]
	public class PlayersSetting
	{
		[SerializeField] private GameObject _playerPrefab;

		public GameObject PlayerPrefab => _playerPrefab;
	}
}