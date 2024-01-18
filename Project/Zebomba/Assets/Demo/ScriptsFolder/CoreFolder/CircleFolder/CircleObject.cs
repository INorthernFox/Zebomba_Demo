using System;
using Demo.ScriptsFolder.CoreFolder.CircleFolder.VisualFolder;
using Demo.ScriptsFolder.DataFolder.CirclesDataFolder;
using UnityEngine;

namespace Demo.ScriptsFolder.CoreFolder.CircleFolder
{
	public class CircleObject : MonoBehaviour
	{
		[SerializeField] private CircleVisual _circleVisual;
		[SerializeField] private CirclePhysics _circlePhysics;
		[SerializeField] private ParticleDisappearance _particleDisappearance;
		
		public CircleVisual CircleVisual => _circleVisual;
		public CirclePhysics CirclePhysics => _circlePhysics;
		public ParticleDisappearance ParticleDisappearance => _particleDisappearance;

		public event Action<CircleObject> OnReset;
		
		public string ID { get; private set; }
		public int Price { get; private set; }

		public void Initialization(CircleConfigurationData configurationData)
		{
			_circleVisual.Initialization(configurationData);
			ID = configurationData.ID;
			Price = configurationData.Price;
		}

		public void Reset()
		{
			OnReset?.Invoke(this);
			
			transform.localScale = Vector3.one;
			transform.localRotation = Quaternion.Euler(Vector3.zero);
			
			ParticleDisappearance.transform.SetParent(transform);
			ParticleDisappearance.transform.localPosition = Vector3.zero;
			ParticleDisappearance.ResetParticle();
		}
	}

}