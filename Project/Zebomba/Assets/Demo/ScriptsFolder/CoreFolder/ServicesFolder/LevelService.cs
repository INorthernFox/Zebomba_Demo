using Demo.ScriptsFolder.CoreFolder.LevelFolder;
using DG.Tweening;
using UnityEngine;

namespace Demo.ScriptsFolder.CoreFolder
{
	public class LevelService
	{
		private readonly GameStateMachine _gameStateMachine;
		private readonly LevelObject _levelObject;

		public LevelObject LevelObject => _levelObject;

		public LevelService(LevelObject prefab, Transform spawnPoint, GameStateMachine gameStateMachine)
		{
			_gameStateMachine = gameStateMachine;
			_gameStateMachine.StateUpdated += GameStateMachineOnStateUpdated;
			_levelObject = Object.Instantiate(prefab, spawnPoint);
		}
		
		private void GameStateMachineOnStateUpdated(GameState state)
		{
			if(state == GameState.Game)
				ShoveLevel();
		}

		public void ShoveLevel()
		{
			_levelObject.transform.DOMove(Vector3.zero, 1.5f);
			_levelObject.Activate();
		}

		public void HideLevel() =>
			_levelObject.transform.DOLocalMove(Vector3.zero, 1.5f);
	}
}