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
		[SerializeField] private LayerMask  _groundMask;
		[SerializeField] private GameObject _playerPrefab;
		[SerializeField] private float      _rotationSpeed = 0.05f;
		[SerializeField] private float      _moveSpeed     = 1.5f;

		public GameObject PlayerPrefab  => _playerPrefab;
		public LayerMask  GroundMask    => _groundMask;
		public float      RotationSpeed => _rotationSpeed;
		public float      MoveSpeed     => _moveSpeed;
	}
}