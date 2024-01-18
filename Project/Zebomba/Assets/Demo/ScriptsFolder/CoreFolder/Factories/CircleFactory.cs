using Demo.ScriptsFolder.CoreFolder.CircleFolder;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Demo.ScriptsFolder.CoreFolder.Factories
{
    public class CircleFactory
    {
        private readonly Transform _baseRoot;
        private readonly CircleObject _prefab;

        public CircleFactory(Transform root, CircleObject prefab)
        {
            _baseRoot = root;
            _prefab = prefab;
        }

        public CircleObject Create()
            => Object.Instantiate( _prefab, _baseRoot);
    }
}
