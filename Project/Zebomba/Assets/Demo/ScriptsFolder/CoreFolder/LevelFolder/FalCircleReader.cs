using System;
using Cysharp.Threading.Tasks;
using Demo.ScriptsFolder.CoreFolder.CircleFolder;
using Demo.ScriptsFolder.DataFolder;
using UnityEngine;

namespace Demo.ScriptsFolder.CoreFolder.LevelFolder
{
	[RequireComponent(typeof(BoxCollider2D))]
	public class FalCircleReader : MonoBehaviour
	{
		[SerializeField] private BoxCollider2D _boxCollider2D;

		public event Action<CircleObject, FalCircleReader> OnCircleCollected;

		private void OnTriggerEnter2D(Collider2D collision)
		{
			if(!collision.TryGetComponent(out CircleObject circleObject))
				return;

			AwaitFill(circleObject);

		}

		private async void AwaitFill(CircleObject circleObject)
		{
			while(Constants.Circle.MinimumEndSpeedFall <= circleObject.CirclePhysics.Speed && circleObject.gameObject.activeSelf)
			{
				await UniTask.Yield();
			}
			
			if(circleObject.gameObject.activeSelf)
				OnCircleCollected?.Invoke(circleObject, this);
		}
	}

}