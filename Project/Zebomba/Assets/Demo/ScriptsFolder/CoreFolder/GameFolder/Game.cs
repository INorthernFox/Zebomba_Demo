using UnityEngine;

namespace Demo.ScriptsFolder.CoreFolder
{
	public class Game
	{
		private readonly CirclePlaceService _circlePlaceService;
		private readonly GameStateMachine _gameStateMachine;
		private readonly MainMenuAnimation _mainMenuAnimation;
		private CircleFillService _circleFillService;

		public Game(CirclePlaceService circlePlaceService, GameStateMachine gameStateMachine, MainMenuAnimation mainMenuAnimation)
		{
			_circlePlaceService = circlePlaceService;
			_gameStateMachine = gameStateMachine;
			_mainMenuAnimation = mainMenuAnimation;
			_gameStateMachine.StateUpdated += GameStateMachineOnStateUpdated;
			StartGame();
		}
		
		private void GameStateMachineOnStateUpdated(GameState state)
		{
			if(state == GameState.AfterRestart)
				StartGameAfterRestart();
		}

		private void StartGame()
		{
			_circlePlaceService.Placed += CirclePlaceServiceOnPlaced;
			_circlePlaceService.TryPlace();
			
		}
		
		private void StartGameAfterRestart()
		{
			_circlePlaceService.Placed += CirclePlaceAfterRestart;
			_circlePlaceService.TryPlace();
			
		}

		private void CirclePlaceServiceOnPlaced()
		{
			_circlePlaceService.Placed -= CirclePlaceServiceOnPlaced;
			_gameStateMachine.TrySetState(GameState.MainMenu);
			_mainMenuAnimation.Generate();
		}
		
		private void CirclePlaceAfterRestart()
		{
			_circlePlaceService.Placed -= CirclePlaceAfterRestart;
			_gameStateMachine.TrySetState(GameState.Game);
		}


	}
}