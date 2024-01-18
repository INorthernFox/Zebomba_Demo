using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using Demo.ScriptsFolder.CoreFolder.CircleFolder;
using Demo.ScriptsFolder.DataFolder;
using UnityEngine;

namespace Demo.ScriptsFolder.CoreFolder.LevelFolder
{
	[RequireComponent(typeof(BoxCollider2D))]
	public class OvercrowdingTrigger : MonoBehaviour
	{
		[SerializeField] private BoxCollider2D _boxCollider2D;

		public event Action<CircleObject> OnCircleCollected;

		private readonly HashSet<CircleObject> _circleObjects = new HashSet<CircleObject>();
		private readonly HashSet<CircleObject> _leftObjects = new HashSet<CircleObject>();

		private void OnTriggerEnter2D(Collider2D collision)
		{
			if(!collision.TryGetComponent(out CircleObject circleObject))
				return;

			if(_circleObjects.Contains(circleObject))
				return;

			_circleObjects.Add(circleObject);
			AwaitFill(circleObject);

		}

		private void OnTriggerExit2D(Collider2D collision)
		{
			if(!collision.TryGetComponent(out CircleObject circleObject))
				return;

			if(!_leftObjects.Contains(circleObject))
				_leftObjects.Add(circleObject);
		}

		private async void AwaitFill(CircleObject circleObject)
		{
			while(Constants.Circle.MinimumEndSpeedFall <= circleObject.CirclePhysics.Speed)
			{
				await UniTask.Yield();
			}

			if(!_leftObjects.Contains(circleObject))
			{
				_circleObjects.Remove(circleObject);
				OnCircleCollected?.Invoke(circleObject);

			}
			else
			{
				_leftObjects.Remove(circleObject);
				_circleObjects.Remove(circleObject);
			}
		}
	}
}