using UnityEngine.SceneManagement;

namespace Demo.ScriptsFolder.CoreFolder
{
	public class ReloadSceneService
	{
		private readonly GameStateMachine _gameStateMachine;
		
		public ReloadSceneService(GameStateMachine gameStateMachine)
		{
			_gameStateMachine = gameStateMachine;
			_gameStateMachine.StateUpdated += OnStateUpdated;
		}
		
		private void OnStateUpdated(GameState state)
		{
			if(state == GameState.Reload)
				SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
		}
	}
}