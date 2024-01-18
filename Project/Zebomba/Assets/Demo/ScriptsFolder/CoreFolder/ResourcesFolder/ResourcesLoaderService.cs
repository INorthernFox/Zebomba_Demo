using System;
using Cysharp.Threading.Tasks;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Demo.ScriptsFolder.CoreFolder.ResourcesFolder
{
	public class ResourcesLoaderService : IResourcesLoaderService
	{
		public async UniTask<T> LoadAsync<T>(string path) where T : Object
		{
			ResourceRequest resourceRequest = Resources.LoadAsync<T>(path);
			await resourceRequest;

			if(resourceRequest.asset is not T prefab)
				throw new ArgumentException($"[{typeof(ResourcesLoaderService)}] Does not match the type {typeof(T)} path {path}");

			if(prefab == null)
				throw new NullReferenceException($"[{typeof(ResourcesLoaderService)}] Prefab");

			return prefab;
		}
		
		public T Load<T>(string path) where T : Object
		{
			if(Resources.Load<T>(path) is not T prefab)
				throw new ArgumentException($"[{typeof(ResourcesLoaderService)}] Does not match the type");

			if(prefab == null)
				throw new NullReferenceException($"[{typeof(ResourcesLoaderService)}] Prefab");

			return prefab;
		}
	}
}