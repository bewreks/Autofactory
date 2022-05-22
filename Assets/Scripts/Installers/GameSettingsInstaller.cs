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
			settings.Prepare();
			Container.Bind<GameSettings>().FromInstance(settings).AsSingle();
		}
	}

	[Serializable]
	public class GameSettings
	{
		[Header("Layers game settings")]
		[SerializeField] private LayerMask _groundMask;

		[SerializeField] private LayerMask _electricPoleMask;
		[SerializeField] private LayerMask _previewLayer;
		[SerializeField] private LayerMask _poleLayer;
		[SerializeField] private LayerMask _generatorLayer;
		[SerializeField] private LayerMask _consumptionLayer;
		[SerializeField] private LayerMask _defaultLayer;

		[Header("Player game settings")]
		[SerializeField] private GameObject _playerPrefab;

		[SerializeField] private float _rotationSpeed = 0.05f;
		[SerializeField] private float _moveSpeed     = 1.5f;


		public GameObject PlayerPrefab     => _playerPrefab;
		public LayerMask  GroundMask       => _groundMask;
		public int        PreviewLayer     { get; private set; }
		public int        PoleLayer        { get; private set; }
		public int        GeneratorLayer   { get; private set; }
		public int        ConsumptionLayer { get; private set; }
		public int        DefaultLayer     { get; private set; }
		public float      RotationSpeed    => _rotationSpeed;
		public float      MoveSpeed        => _moveSpeed;
		public LayerMask  ElectricPoleMask => _electricPoleMask;

		public void Prepare()
		{
			PreviewLayer     = (int)Mathf.Log(_previewLayer.value,     2);
			PoleLayer        = (int)Mathf.Log(_poleLayer.value,        2);
			GeneratorLayer   = (int)Mathf.Log(_generatorLayer.value,   2);
			ConsumptionLayer = (int)Mathf.Log(_consumptionLayer.value, 2);
			DefaultLayer     = (int)Mathf.Log(_defaultLayer.value,     2);
		}
	}
}