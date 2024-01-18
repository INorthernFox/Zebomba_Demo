using System;
using Cysharp.Threading.Tasks;
using JetBrains.Annotations;
using UnityEngine;

namespace Demo.ScriptsFolder.CoreFolder
{
	public class InputService : IDisposable
	{
		private bool _isActive;

		[CanBeNull] public event Action Click;

		private int index = 0;

		private bool _dispose = false;

		public InputService()
		{
			Update();
		}

		public void Active() =>
			_isActive = true;

		private async void Update()
		{
			while(!_dispose)
			{
				await UniTask.Yield(PlayerLoopTiming.Update);
				
				if(Input.GetMouseButtonDown(0) && _isActive)
					Click?.Invoke();
			}
		}

		public void Deactivate() =>
			_isActive = false;

		public void Dispose() =>
			_dispose = true;
	}
}