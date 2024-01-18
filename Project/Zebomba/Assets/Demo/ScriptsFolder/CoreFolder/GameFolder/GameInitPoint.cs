using Demo.ScriptsFolder.CoreFolder.CircleFolder;
using Demo.ScriptsFolder.CoreFolder.CircleFolder.VisualFolder;
using Demo.ScriptsFolder.CoreFolder.LevelFolder;
using Demo.ScriptsFolder.CoreFolder.ResourcesFolder;
using Demo.ScriptsFolder.DataFolder;
using Demo.ScriptsFolder.DataFolder.CirclesDataFolder;
using Demo.ScriptsFolder.UIFolder;
using UnityEngine;

namespace Demo.ScriptsFolder.CoreFolder
{
	public class GameInitPoint : MonoBehaviour
	{
		[SerializeField] private Transform _pendulumSpawnPoint;
		[SerializeField] private Transform _levelSpawnPoint;
		[SerializeField] private Camera _camera;

		private readonly ResourcesLoaderService _resourcesLoader = new ResourcesLoaderService();

		private DisposableService _disposableService;

		private void Awake()
		{
			Application.targetFrameRate = 300;
			
			PendulumObject pendulumObject = Instantiate(_resourcesLoader.Load<PendulumObject>(Constants.Level.PendulumResourcesPrefab), _pendulumSpawnPoint);
			DisappearanceAnimator disappearanceAnimator = new DisappearanceAnimator();
			GameStateMachine gameStateMachine = new GameStateMachine();

			LevelObject levelPrefab = _resourcesLoader.Load<LevelObject>(Constants.Level.TestLevelResourcesPrefab);
			LevelService levelService = new LevelService(levelPrefab, _levelSpawnPoint, gameStateMachine);

			CircleConfiguration circleConfiguration = _resourcesLoader.Load<CircleConfiguration>(Constants.Circle.CircleConfigurationCaseResourcesPath);
			CircleObject circleObject = _resourcesLoader.Load<CircleObject>(Constants.Circle.CircleObjectResourcesPrefab);

			CircleService circleService = new CircleService(circleConfiguration, circleObject);
			CircleDisappearanceService circleDisappearanceService = new CircleDisappearanceService(circleService, disappearanceAnimator);
			CirclePlaceService circlePlaceService = new CirclePlaceService(circleService, pendulumObject);
			CircleFillService circleFillService = new CircleFillService(levelService.LevelObject, circleDisappearanceService, gameStateMachine);

			InputService inputService = new InputService();
			ScoreService scoreService = new ScoreService();
			ScoreAccrualService scoreAccrualService = new ScoreAccrualService(scoreService, circleFillService);

			GameCanvas gameCanvas = _resourcesLoader.Load<GameCanvas>(Constants.UI.GameCanvasResourcesPrefab);
			UIService uiService = new UIService(gameCanvas, scoreService, gameStateMachine);

			RestartService restartService = new RestartService(gameStateMachine);
			restartService.AddRestartObject(circleService);
			restartService.AddRestartObject(circleFillService);
			restartService.AddRestartObject(scoreService);
			restartService.AddRestartObject(pendulumObject.CircleCapture);
			restartService.AddRestartObject(circlePlaceService);

			ReloadSceneService reloadSceneService = new ReloadSceneService(gameStateMachine);
			ThrowingService throwingService = new ThrowingService(inputService, gameStateMachine, circlePlaceService, pendulumObject);

			MainMenuAnimation mainMenuAnimation = new MainMenuAnimation(circleService, _camera, gameStateMachine, disappearanceAnimator);

			_disposableService = new DisposableService();
			_disposableService.Add(inputService);
			
			Game game = new Game(circlePlaceService, gameStateMachine, mainMenuAnimation);
		}

		private void OnDestroy()
		{
			_disposableService.Dispose();
		}
	}
}