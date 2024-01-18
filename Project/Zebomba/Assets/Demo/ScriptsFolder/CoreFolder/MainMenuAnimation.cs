using System.Collections.Generic;
using System.Linq;
using Cysharp.Threading.Tasks;
using Demo.ScriptsFolder.CoreFolder.CircleFolder;
using Demo.ScriptsFolder.CoreFolder.CircleFolder.VisualFolder;
using UnityEngine;

namespace Demo.ScriptsFolder.CoreFolder
{
	public class MainMenuAnimation
	{
		private readonly CircleService _circleService;
		private readonly Camera _camera;
		private readonly GameStateMachine _gameStateMachine;

		private int _minSpawnTime = 10;
		private int _maxSpawnTime = 150;
		
		private int _minLiveTime = 200;
		private int _maxLiveTime = 1000;
		
		private readonly HashSet<CircleObject> _objects = new HashSet<CircleObject>();

		private readonly DisappearanceAnimator _disappearanceAnimator;

		private bool AnimationIsPossible => _gameStateMachine.GameState == GameState.MainMenu;

		public MainMenuAnimation(CircleService circleService, Camera camera, GameStateMachine gameStateMachine, DisappearanceAnimator disappearanceAnimator)
		{
			_circleService = circleService;
			_camera = camera;
			_gameStateMachine = gameStateMachine;
			_disappearanceAnimator = disappearanceAnimator;
		}

		public async void Generate()
		{
			CreateCircle();
			int time = Random.Range(_minSpawnTime, _maxSpawnTime);
			await UniTask.Delay(time);

			if(AnimationIsPossible)
				Generate();
			else
				StopAnimation();
		}


		private async void CreateCircle()
		{
			CircleObject circleObject = _circleService.GetCircle();
			circleObject.transform.position = GetPont();
			circleObject.gameObject.SetActive(true);
			circleObject.CirclePhysics.SetRandomGravity();

			Vector2 force = Random.insideUnitCircle;
			circleObject.CirclePhysics.AddForce(force);

			_objects.Add(circleObject);

			int time = Random.Range(_minLiveTime, _maxLiveTime);
			await UniTask.Delay(time);

			if(AnimationIsPossible)
			{
				circleObject.CirclePhysics.DisablePhysics();
				_disappearanceAnimator.Animate(circleObject, OnEndAnimation);
			}
		}

		private void StopAnimation()
		{
			CircleObject[] circleObjects = _objects.ToArray();

			for( int i = 0; i < circleObjects.Length; i++ )
				_circleService.ReturnCircle(circleObjects[i]);

			_objects.Clear();
		}

		private void OnEndAnimation(CircleObject circleObject)
		{
			_objects.Remove(circleObject);
			_circleService.ReturnCircle(circleObject);
		}

		private Vector3 GetPont()
		{
			float screenWidth = Screen.width;
			float screenHeight = Screen.height;

			float randomX = Random.Range(0, screenWidth);
			float randomY = Random.Range(0, screenHeight);

			Vector3 randomPosition = _camera.ScreenToWorldPoint(new Vector3(randomX, randomY, 10f));

			return randomPosition;
		}
	}
}