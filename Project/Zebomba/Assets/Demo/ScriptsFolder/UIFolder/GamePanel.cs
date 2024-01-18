using Demo.ScriptsFolder.CoreFolder;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Demo.ScriptsFolder.UIFolder
{
	public class GamePanel : BasePanel
	{
		[SerializeField] private TMP_Text _score;
		[SerializeField] private Button _restartButton;
		
		private ScoreService _scoreService;
		private GameStateMachine _gameStateMachine;

		public void Initialization(ScoreService scoreService, GameStateMachine gameStateMachine)
		{
			_gameStateMachine = gameStateMachine;
			_scoreService = scoreService;
			_scoreService.Updated += ScoreUpdated;
			ScoreUpdated(_scoreService.Score);
			
			_restartButton.onClick.AddListener(Restart);
		}
		
		private void Restart() =>
			_gameStateMachine.TrySetState(GameState.Restart);

		private void ScoreUpdated(int value) =>
			_score.text = $"Score {value}";
	}
}