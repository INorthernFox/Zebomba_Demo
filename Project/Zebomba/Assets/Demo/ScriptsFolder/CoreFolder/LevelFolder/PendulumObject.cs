
using System;
using DG.Tweening;
using UnityEngine;

namespace Demo.ScriptsFolder.CoreFolder.LevelFolder
{
	public class PendulumObject : MonoBehaviour
	{
		[SerializeField] private PendulumRope _rope;
		[SerializeField] private PendulumCircleCapture _capture;

		[SerializeField] private float _limitAngle;
		[SerializeField] private float _pendulumWorkingHeight;

		[SerializeField] private float _speed;
		[SerializeField] private AnimationCurve _speedModifierFromProximityToCent;

		[SerializeField] private float _hideTime;
		[SerializeField] private float _shoveTime;
		[SerializeField] private AnimationCurve _shoveEase;
		
		private DirectionRotation _directionRotation = DirectionRotation.Clockwise;
		private float _currentAngle = 0;

		private Tweener _activeTwiner;

		public event Action OnHide;
		public event Action OnShove;
		
		public PendulumCircleCapture CircleCapture => _capture;
		public PendulumRope PendulumRope => _rope;

		private void FixedUpdate() =>
			PendulumLogics();

		public void SetSpeed(float speed) =>
			_speed = speed;

		public void Hide()
		{
			_activeTwiner?.Kill();
			_activeTwiner = _capture.transform.DOLocalMove(Vector3.zero, _hideTime).SetEase(Ease.Linear).OnComplete(() => OnHide?.Invoke());
		}

		public void Shove()
		{
			_activeTwiner?.Kill();
			_activeTwiner = _capture.transform.DOLocalMove(Vector3.up * _pendulumWorkingHeight, _shoveTime).SetEase(_shoveEase).OnComplete(() => OnShove?.Invoke());
		}

		private void PendulumLogics()
		{
			float speedModifier =  _speedModifierFromProximityToCent.Evaluate(Math.Abs(_currentAngle) / _limitAngle);
			float angle = _speed * speedModifier  * Time.deltaTime;
			
			if(_directionRotation == DirectionRotation.Clockwise)
			{
				transform.Rotate(Vector3.back, angle);
				_currentAngle -= angle;
			}
			else
			{
				transform.Rotate(Vector3.forward, angle);
				_currentAngle += angle;
			}
			
			if(_directionRotation == DirectionRotation.Clockwise && _currentAngle < -_limitAngle)
				_directionRotation = DirectionRotation.Counterclockwise;
			else if(_directionRotation == DirectionRotation.Counterclockwise && _currentAngle > _limitAngle)
				_directionRotation = DirectionRotation.Clockwise;
		}
		
		private enum DirectionRotation
		{
			Clockwise = 0,
			Counterclockwise = 1,
		}
	}
}