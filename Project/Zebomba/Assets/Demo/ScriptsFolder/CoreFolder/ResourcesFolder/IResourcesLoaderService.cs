using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Demo.ScriptsFolder.CoreFolder.ResourcesFolder
{
	public interface IResourcesLoaderService
	{
		UniTask<T> LoadAsync<T>(string path) where T : Object;
		T Load<T>(string path) where T : Object;
	}
}