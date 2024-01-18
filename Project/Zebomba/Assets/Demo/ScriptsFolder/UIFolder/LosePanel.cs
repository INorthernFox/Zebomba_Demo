using Demo.ScriptsFolder.CoreFolder;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Demo.ScriptsFolder.UIFolder
{
    public class LosePanel : BasePanel
    {
        [SerializeField] private Button _menuButton;
        [SerializeField] private Button _restartButton;
        [SerializeField] private TMP_Text _score;
        
        private ScoreService _scoreService;
        private GameStateMachine _gameStateMachine;

        public void Initialization(ScoreService scoreService, GameStateMachine gameStateMachine)
        {
            _scoreService = scoreService;
            _gameStateMachine = gameStateMachine;
            _restartButton.onClick.AddListener(OnRestart);
            _menuButton.onClick.AddListener(OnMenu);
        }
        
        private void OnMenu() =>
            _gameStateMachine.TrySetState(GameState.Reload);

        private void OnRestart() =>
            _gameStateMachine.TrySetState(GameState.Restart);

        public override void Open()
        {
            _score.text = $"Score {_scoreService.Score}";
            base.Open();
        }
    }

}
