using System;
using Demo.ScriptsFolder.CoreFolder.CircleFolder;
using Demo.ScriptsFolder.CoreFolder.LevelFolder;
using UnityEngine;

namespace Demo.ScriptsFolder.CoreFolder
{
	public class CirclePlaceService : IRestarted
	{
		private readonly CircleService _circleService;
		private readonly PendulumObject _pendulumObject;

		public event Action Placed;
		
		public CirclePlaceService(CircleService circleService, PendulumObject pendulumObject)
		{
			_circleService = circleService;
			_pendulumObject = pendulumObject;
		}

		public bool TryPlace()
		{
			if(_pendulumObject.CircleCapture.HaveObject)
				return false;
			
			_pendulumObject.OnHide += OnPendulumHide;
			_pendulumObject.Hide();
			
			return true;
		}
		
		private void OnPendulumHide()
		{
			_pendulumObject.OnHide -= OnPendulumHide;
			
			CircleObject circleObject = _circleService.GetCircle();
			_pendulumObject.CircleCapture.FreezeObject(circleObject);
			_pendulumObject.Shove();
			Placed?.Invoke();
		}
		
		
		public void Restart()
		{
			_pendulumObject.OnHide -= OnPendulumHide;
		}
	}
}