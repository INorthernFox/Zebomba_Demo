using System;
using Demo.ScriptsFolder.CoreFolder.CircleFolder;
using UnityEngine;

namespace Demo.ScriptsFolder.CoreFolder.LevelFolder
{
	public class PendulumCircleCapture : MonoBehaviour, IRestarted
	{
		private CircleObject _circleObject;

		private Vector3 _motionVector;
		private Vector3 _lastPosition;

		public bool HaveObject => _circleObject != null;
		
		private void FixedUpdate()
		{
			RefreshMotionData();
		}

		public void FreezeObject(CircleObject circleObject)
		{
			if(_circleObject != null)
				throw new ArgumentException("There is already a captured object");
			
			_circleObject = circleObject;
			_circleObject.transform.SetParent(transform);
			_circleObject.transform.localPosition = Vector3.zero;
			_circleObject.transform.rotation = transform.rotation;
			_circleObject.CirclePhysics.DisablePhysics();
		}

		public void ReleaseObject()
		{
			if(_circleObject == null)
				throw new ArgumentException("The captured object is null");
			
			_circleObject.transform.SetParent(null);
			_circleObject.CirclePhysics.EnablePhysics();
			_circleObject.CirclePhysics.AddForce(_motionVector);
			_circleObject = null;
		}

		private void RefreshMotionData()
		{
			_motionVector = transform.position - _lastPosition;
			_lastPosition = transform.position;
		}
		
		public void Restart() =>
			_circleObject = null;
	}
}