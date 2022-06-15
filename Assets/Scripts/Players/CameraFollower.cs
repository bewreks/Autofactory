using System;
using UnityEngine;

namespace Players
{
	public class CameraFollower : MonoBehaviour
	{
		public GameObject follow;

		private void Update()
		{
			if (follow)
			{
				transform.position = follow.transform.position + new Vector3(2.21f, 5.7f, -6.17f);
			}
		}
	}
}