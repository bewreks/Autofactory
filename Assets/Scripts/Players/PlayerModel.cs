using System;
using Inventories;
using UnityEngine;

namespace Players
{
	public class PlayerModel : MonoBehaviour, IDisposable
	{
		public IInventory Inventory { get; private set; }

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