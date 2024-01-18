using System.Linq;
using Demo.ScriptsFolder.CoreFolder.CircleFolder;
using Demo.ScriptsFolder.CoreFolder.CircleFolder.VisualFolder;

namespace Demo.ScriptsFolder.CoreFolder
{
	public class CircleDisappearanceService
	{
		private readonly CircleService _circleService;
		private readonly DisappearanceAnimator _disappearanceAnimator;

		public CircleDisappearanceService(CircleService circleService, DisappearanceAnimator disappearanceAnimator)
		{
			_circleService = circleService;
			_disappearanceAnimator = disappearanceAnimator;
		}

		public void Disappearance(CircleObject[] circleObjects)
		{
			foreach(CircleObject circleObject in circleObjects.Where(p => p != null))
				_disappearanceAnimator.Animate(circleObject, OnDisappearance);
		}

		private void OnDisappearance(CircleObject circleObject) =>
			_circleService.ReturnCircle(circleObject);
	}
}