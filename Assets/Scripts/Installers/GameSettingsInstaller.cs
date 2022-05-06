using System;
using UnityEngine;
using Zenject;

namespace Installers
{
	[CreateAssetMenu(fileName = "GameSettingsInstaller", menuName = "Installers/GameSettingsInstaller")]
	public class GameSettingsInstaller : ScriptableObjectInstaller<GameSettingsInstaller>
	{
		[SerializeField] private GameSettings settings;

		public override void InstallBindings()
		{
			Container.Bind<GameSettings>().FromInstance(settings).AsSingle();
		}
	}

	[Serializable]
	public class GameSettings
	{
		[Header("Global game settings")]
		[SerializeField] private LayerMask  _groundMask;
		[Header("Player game settings")]
		[SerializeField] private GameObject _playerPrefab;
		[SerializeField] private float      _rotationSpeed = 0.05f;
		[SerializeField] private float      _moveSpeed     = 1.5f;

		public GameObject PlayerPrefab  => _playerPrefab;
		public LayerMask  GroundMask    => _groundMask;
		public float      RotationSpeed => _rotationSpeed;
		public float      MoveSpeed     => _moveSpeed;
	}
}