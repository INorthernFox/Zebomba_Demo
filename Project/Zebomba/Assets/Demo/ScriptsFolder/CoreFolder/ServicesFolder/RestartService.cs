using System.Collections.Generic;
using Cysharp.Threading.Tasks;

namespace Demo.ScriptsFolder.CoreFolder
{
	public class RestartService
	{
		private readonly GameStateMachine _gameStateMachine;
		private readonly List<IRestarted> _list = new List<IRestarted>();

		public RestartService(GameStateMachine gameStateMachine)
		{
			_gameStateMachine = gameStateMachine;
			_gameStateMachine.StateUpdated += GameStateMachineOnStateUpdated;

		}
		
		private void GameStateMachineOnStateUpdated(GameState state)
		{
			if(state == GameState.Restart)
				Restart();
		}
		
		private async void Restart()
		{
			foreach(IRestarted target in _list)
				target.Restart();

			await UniTask.Yield(PlayerLoopTiming.LastUpdate);
			_gameStateMachine.TrySetState(GameState.AfterRestart);
		}

		public void AddRestartObject(IRestarted restarted)
		{
			if(_list.Contains(restarted))
				return;
			
			_list.Add(restarted);
		}
	}
}