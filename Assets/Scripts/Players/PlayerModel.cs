using System;
using Inventories;
using UnityEngine;

namespace Players
{
	public class PlayerModel : MonoBehaviour, IDisposable
	{
		[SerializeField] private GameObject playerViewModel;
		
		public IInventory Inventory       { get; private set; }
		public GameObject PlayerViewModel => playerViewModel;

		private void Awake()
		{
			Inventory = new Inventory();
		}

		public void Dispose()
		{
			Inventory?.Dispose();
		}
	}
}