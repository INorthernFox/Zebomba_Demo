using System.Linq;
using Demo.ScriptsFolder.CoreFolder.CircleFolder;

namespace Demo.ScriptsFolder.CoreFolder
{
	public class ScoreAccrualService
	{
		private readonly ScoreService _scoreService;
		private readonly CircleFillService _circleFillService;

		public ScoreAccrualService(ScoreService scoreService, CircleFillService circleFillService)
		{
			_scoreService = scoreService;
			_circleFillService = circleFillService;
			_circleFillService.CircleCollected += OnCircleCollected;
		}

		private void OnCircleCollected(CircleObject[] circles)
		{
			int value = circles.Sum(circle => circle.Price);
			_scoreService.Add(value);
		}
	}
}