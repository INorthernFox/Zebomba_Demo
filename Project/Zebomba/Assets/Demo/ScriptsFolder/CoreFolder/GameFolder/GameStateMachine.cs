using System;

namespace Demo.ScriptsFolder.CoreFolder
{
	public class GameStateMachine
	{
		private GameState _gameState = GameState.Non;
		
		public event Action<GameState> StateUpdated;

		public GameState GameState => _gameState;
		
		public bool TrySetState(GameState gameState)
		{
			if(gameState == _gameState)
				return false;

			_gameState = gameState;
			StateUpdated?.Invoke(_gameState);
			return true;
		}
	}
}