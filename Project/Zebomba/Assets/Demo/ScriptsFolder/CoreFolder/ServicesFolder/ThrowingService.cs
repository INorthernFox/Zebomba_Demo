using System;
using Demo.ScriptsFolder.CoreFolder.LevelFolder;
using UnityEngine;

namespace Demo.ScriptsFolder.CoreFolder
{
	public class ThrowingService
	{
		private readonly InputService _inputService;
		private readonly GameStateMachine _gameStateMachine;
		private readonly PendulumObject _pendulumObject;
		private readonly CirclePlaceService _circlePlaceService;

		public ThrowingService( InputService inputService, GameStateMachine gameStateMachine,CirclePlaceService circlePlaceService , PendulumObject pendulumObject)
		{
			_inputService = inputService;
			_gameStateMachine = gameStateMachine;
			_pendulumObject = pendulumObject;
			_circlePlaceService = circlePlaceService;
			_inputService.Click += InputServiceOnClick;
			_gameStateMachine.StateUpdated += OnStateUpdated;
		}
		
		private void OnStateUpdated(GameState state)
		{
			switch(state)
			{
				case GameState.Non:
					_inputService.Deactivate();
					break;
				case GameState.MainMenu:
					_inputService.Active();
					break;
				case GameState.Game:
					_inputService.Active();
					break;
				case GameState.EndGame:
					_inputService.Deactivate();
					break;
				case GameState.Restart:
					_inputService.Deactivate();
					break;
				case GameState.Reload:
					_inputService.Deactivate();
					break;
				case GameState.AfterRestart:
					_inputService.Deactivate();
					break;
				default:
					throw new ArgumentOutOfRangeException(nameof(state), state, null);
			}
		}

		private void InputServiceOnClick()
		{
			if(_gameStateMachine.GameState == GameState.MainMenu)
				_gameStateMachine.TrySetState(GameState.Game);
			
			if(!_pendulumObject.CircleCapture.HaveObject)
				return;
		
			_inputService.Deactivate();
			_pendulumObject.CircleCapture.ReleaseObject();
			
			_circlePlaceService.Placed += OnPlaced;
			_circlePlaceService.TryPlace();
		}
		
		private void OnPlaced()
		{
			_circlePlaceService.Placed -= OnPlaced;
			_inputService.Active();
		}
	}
}