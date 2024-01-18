using System;
using Demo.ScriptsFolder.CoreFolder;
using UnityEngine;

namespace Demo.ScriptsFolder.UIFolder
{
	public class GameCanvas : MonoBehaviour
	{
		private BasePanel _activePanel;

		[SerializeField] private MainMenuPanel _menuPanel;
		[SerializeField] private GamePanel _gamePanel;
		[SerializeField] private LosePanel _losePanel;

		public void Initialization(ScoreService scoreService, GameStateMachine gameStateMachine)
		{
			_gamePanel.Initialization(scoreService, gameStateMachine);
			_losePanel.Initialization(scoreService, gameStateMachine);
		}

		public void UpdateState(GameState gameState)
		{
			BasePanel newPanel = gameState switch
			{
				GameState.MainMenu => _menuPanel,
				GameState.Game => _gamePanel,
				GameState.EndGame => _losePanel,
				GameState.Non => null,
				GameState.Restart => null,
				GameState.AfterRestart => null,
				GameState.Reload => null,
				_ => throw new ArgumentOutOfRangeException(nameof(gameState), gameState, null)
			};

			if(newPanel == _activePanel)
				return;

			if(_activePanel != null)
				_activePanel.Close();

			_activePanel = newPanel;
			
			if(newPanel != null)
				_activePanel.Open();

		}
	}
}