using System;
using UnityEngine;

namespace Demo.ScriptsFolder.CoreFolder.LevelFolder
{
	public class PendulumRope : MonoBehaviour
	{
		[SerializeField] private LineRenderer _lineRenderer;

		[SerializeField] private Transform _start;
		[SerializeField] private Transform _end;

		private void Awake()
		{
			_lineRenderer.positionCount = 2;
			UpdatePositions();
		}

		private void LateUpdate()
		{
			UpdatePositions();
		}

		private void UpdatePositions()
		{
			_lineRenderer.SetPosition(0, _start.position);
			_lineRenderer.SetPosition(1, _end.position);
		}

		private void OnValidate()
		{
			_lineRenderer.positionCount = 2;
			UpdatePositions();
		}
	}

}