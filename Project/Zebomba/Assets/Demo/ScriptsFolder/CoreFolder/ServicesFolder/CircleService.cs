using System.Collections.Generic;
using System.Linq;
using Demo.ScriptsFolder.CoreFolder.CircleFolder;
using Demo.ScriptsFolder.CoreFolder.Factories;
using Demo.ScriptsFolder.DataFolder.CirclesDataFolder;
using UnityEngine;

namespace Demo.ScriptsFolder.CoreFolder
{
	public class CircleService : IRestarted
	{
		private readonly string[] _ids;
		private readonly CirclePool _circlePool;

		private readonly HashSet<CircleObject> _circlesInGame = new HashSet<CircleObject>();

		public CircleService(CircleConfiguration circleConfiguration, CircleObject prefab)
		{
			_ids = circleConfiguration.GetConfigurations().Keys.ToArray();
			GameObject gameObject = new GameObject("CircleCase");
			_circlePool = new CirclePool( gameObject.transform, prefab, circleConfiguration);
		}

		public CircleObject GetCircle()
		{
			string id = GetId();
			CircleObject circleObject = _circlePool.Get(id);
			
			circleObject.gameObject.SetActive(true);
			circleObject.CirclePhysics.EnablePhysics();
			_circlesInGame.Add(circleObject);
			return circleObject;
		}

		public void ReturnCircle(CircleObject circleObject)
		{
			circleObject.gameObject.SetActive(false);
			circleObject.CirclePhysics.DisablePhysics();
			circleObject.CirclePhysics.ResetPhysics();
			circleObject.transform.localScale = Vector3.one;
			circleObject.transform.rotation = Quaternion.Euler(Vector3.zero);
			
			circleObject.Reset();
			
			_circlesInGame.Remove(circleObject);
			_circlePool.Set(circleObject);
		}

		//Здесь должна быть какая-то крутая логика, придуманная ГД
		private string GetId() =>
			_ids[Random.Range(0, _ids.Length)];

		public void Restart()
		{
			CircleObject[] circleObjects = _circlesInGame.ToArray();

			for( int i = 0; i < circleObjects.Length; i++ )
				ReturnCircle(circleObjects[i]);

			_circlesInGame.Clear();
		}
	}

}