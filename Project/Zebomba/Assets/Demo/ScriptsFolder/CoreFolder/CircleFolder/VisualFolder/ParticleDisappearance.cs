using UnityEngine;

namespace Demo.ScriptsFolder.CoreFolder.CircleFolder.VisualFolder
{
	public class ParticleDisappearance : MonoBehaviour
	{
		[SerializeField] private ParticleSystem[] _colorChanged;
		[SerializeField] private ParticleSystem[] _all;
		[SerializeField] private ParticleSystem _main;

		public void ResetParticle()
		{
			foreach(ParticleSystem all in _all)
			{
				all.Stop();
				all.Clear();
			}
		}

		public void Play()
		{
			_main.Play();
		}

		public void SetPosition(Vector3 position)
		{
			_main.transform.position = position;
		}

		public void SetColor(Color color)
		{
			foreach(ParticleSystem colorChange in _colorChanged)
			{
				ParticleSystem.MainModule mainModule = colorChange.main;
				mainModule.startColor = color;
			}
		}

		private void OnValidate() =>
			_all = GetComponentsInChildren<ParticleSystem>(true);
	}

}