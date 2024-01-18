using System;
using Demo.ScriptsFolder.UIFolder;

namespace Demo.ScriptsFolder.CoreFolder
{
	public class UIService : IDisposable
	{
		private readonly GameCanvas _gameCanvas;
		private readonly GameStateMachine _gameStateMachine;

		public UIService(GameCanvas gameCanvas, ScoreService scoreService, GameStateMachine gameStateMachine)
		{
			_gameCanvas = UnityEngine.Object.Instantiate(gameCanvas);
			_gameStateMachine = gameStateMachine;
			_gameStateMachine.StateUpdated += UpdateState;
			UpdateState(_gameStateMachine.GameState);
			_gameCanvas.Initialization(scoreService, gameStateMachine);
		}

		private void UpdateState(GameState gameState) =>
			_gameCanvas.UpdateState(gameState);
		
		public void Dispose() =>
			_gameStateMachine.StateUpdated -= UpdateState;
	}
}