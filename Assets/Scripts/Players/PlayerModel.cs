using System;
using Game;
using Installers;
using Inventories;
using UnityEngine;
using Zenject;

namespace Players
{
	public class PlayerModel : MonoBehaviour, IDisposable
	{
		[Inject] private IGameModel   _model;
		[Inject] private GameSettings _gameSettings;

		[SerializeField] private GameObject playerViewModel;

		public IInventory Inventory       { get; private set; }
		public GameObject PlayerViewModel => playerViewModel;

		private Rigidbody _rigidbody;

		public Rigidbody Rigidbody => _rigidbody;

		private void Awake()
		{
			Inventory  = new Inventory();
			_rigidbody = GetComponent<Rigidbody>();
		}

		public void Dispose()
		{
			Inventory?.Dispose();
		}
	}
}