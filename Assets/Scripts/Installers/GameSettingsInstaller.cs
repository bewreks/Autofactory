using System;
using UnityEngine;
using Zenject;

namespace Game
{
	[CreateAssetMenu(fileName = "GameSettingsInstaller", menuName = "Installers/GameSettingsInstaller")]
	public class GameSettingsInstaller : ScriptableObjectInstaller<GameSettingsInstaller>
	{
		[SerializeField] private GameSettings settingses;
    
		public override void InstallBindings()
		{
			Container.Bind<GameSettings>().FromInstance(settingses).AsSingle();
		}
	}
	
	[Serializable]
	public class GameSettings
	{
		[SerializeField] private GameObject _playerPrefab;
		[SerializeField] private LayerMask  _groundMask;

		public GameObject PlayerPrefab => _playerPrefab;
		public LayerMask  GroundMask   => _groundMask;
	}
}