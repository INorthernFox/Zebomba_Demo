
namespace Demo.ScriptsFolder.DataFolder
{
	public static class Constants
	{
		public static class ScriptableObjectMenu
		{
			public const string BasePath = "Zebomba";
			public const string DataPath = BasePath + "/Data";
		}
        
		public static class Circle
		{
			public const string CircleConfigurationCaseMenuPath = ScriptableObjectMenu.DataPath + "/CircleConfigurationCase";
			public const string CircleConfigurationCaseBaseName = "CircleConfigurationCase";
			
			public const string CircleObjectResourcesPrefab = "Prefabs/Circles/CircleObject";
			public const string CircleConfigurationCaseResourcesPath = "ScriptableObjects/Circles/CircleConfigurationCase";
			public const float MinimumEndSpeedFall = 0.01f;
		}
		
		public static class Level
		{
			public const string TestLevelResourcesPrefab = "Prefabs/Levels/Demo_Level";
			public static string PendulumResourcesPrefab = "Prefabs/Levels/PendulumObject";
		}

		public static class UI
		{
			public const string GameCanvasResourcesPrefab = "Prefabs/UI/GameCanvas";
		}
	}
}