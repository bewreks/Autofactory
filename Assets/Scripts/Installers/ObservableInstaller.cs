using UniRx;
using UnityEngine;
using Zenject;

namespace Installers
{
	[CreateAssetMenu(fileName = "ObservableInstaller", menuName = "Installers/ObservableInstaller")]
	public class ObservableInstaller : ScriptableObjectInstaller<ObservableInstaller>
	{
		public override void InstallBindings()
		{
			var gameObject = new GameObject("Observable", typeof(MonoObservable));
			DontDestroyOnLoad(gameObject);
		}
	}

	public static class ObservableHelper
	{
		public static readonly ReactiveCommand EveryFixedUpdate = new ReactiveCommand();
		
		public static void OnFixedUpdate()
		{
			EveryFixedUpdate.Execute();
		}

		public static void Dispose()
		{
			EveryFixedUpdate.Dispose();
		}
	}

	public class MonoObservable : MonoBehaviour
	{
		private void FixedUpdate()
		{
			ObservableHelper.OnFixedUpdate();
		}

		private void OnDestroy()
		{
			ObservableHelper.Dispose();
		}
	}
}