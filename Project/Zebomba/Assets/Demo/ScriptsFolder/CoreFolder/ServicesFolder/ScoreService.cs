using System;

namespace Demo.ScriptsFolder.CoreFolder
{
	public class ScoreService : IRestarted
	{
		private int _score;

		public event Action<int> Updated;
		public int Score => _score;

		public void Add(int value)
		{
			if(value < 0)
				throw new ArgumentException("Number less than zero");

			_score += value;
			Updated?.Invoke(_score);
		}
		
		public void Restart()
		{
			_score = 0;
			Updated?.Invoke(_score);
		}
	}
}