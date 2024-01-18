using System.Collections.Generic;
using UnityEngine;

namespace Demo.ScriptsFolder.CoreFolder.LevelFolder
{
	public class LevelObject : MonoBehaviour
	{
		[SerializeField] private Transform _wallsRoot;

		[SerializeField] private FalCircleReader[] _falCircleReaders;
		[SerializeField] private OvercrowdingTrigger _overcrowdingTrigger;
		[SerializeField] private int _numberCircleColumn;

		public IReadOnlyCollection<FalCircleReader> FalCircleReaders => _falCircleReaders;
		public OvercrowdingTrigger OvercrowdingTrigger => _overcrowdingTrigger;
		public int NumberCircleColumn => _numberCircleColumn;

		public void Activate()
		{
			foreach(FalCircleReader falCircleReader in _falCircleReaders)
			{
				falCircleReader.gameObject.SetActive(true);
			}

			_overcrowdingTrigger.gameObject.SetActive(true);
		}
	}

}