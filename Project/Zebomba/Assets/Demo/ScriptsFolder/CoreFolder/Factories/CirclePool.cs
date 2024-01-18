using System;
using System.Collections.Generic;
using System.Linq;
using Demo.ScriptsFolder.CoreFolder.CircleFolder;
using Demo.ScriptsFolder.DataFolder.CirclesDataFolder;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Demo.ScriptsFolder.CoreFolder.Factories
{
	public class CirclePool : IDisposable
	{
		private readonly Transform _root;
		private readonly CircleFactory _factory;
		private readonly Queue<CircleObject> _pool;
		private readonly IReadOnlyDictionary<string, CircleConfigurationData> _configurations;

		public CirclePool(Transform root, CircleObject prefabs, CircleConfiguration circleConfiguration)
		{
			_root = root;
			_configurations = circleConfiguration.GetConfigurations();
			_factory = new CircleFactory(root, prefabs);
			_pool = new Queue<CircleObject>();
		}

		public CircleObject Get(string id)
		{
			if(string.IsNullOrEmpty(id))
				throw new ArgumentNullException("Key Is Null Or Empty");

			if(!_configurations.TryGetValue(id, out CircleConfigurationData config))
				throw new ArgumentException($"Key {id} is missing from the configurations");
			
			CircleObject circleObject = _pool.Count > 0 ? _pool.Dequeue() : _factory.Create();
			
			circleObject.Initialization(config);

			return circleObject;
		}

		public void Set(CircleObject circleObject)
		{
			circleObject.transform.SetParent(_root);
			_pool.Enqueue(circleObject);
		}

		public void Dispose()
		{
			while(_pool.Count > 0)
				Object.Destroy(_pool.Dequeue().gameObject);

			_pool.Clear();
		}
	}
}