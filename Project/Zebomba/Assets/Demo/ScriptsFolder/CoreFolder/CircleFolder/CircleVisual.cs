using Demo.ScriptsFolder.CoreFolder.CircleFolder.VisualFolder;
using Demo.ScriptsFolder.DataFolder.CirclesDataFolder;
using UnityEngine;

namespace Demo.ScriptsFolder.CoreFolder.CircleFolder
{
	[RequireComponent(typeof(SpriteRenderer))]
	public class CircleVisual : MonoBehaviour
	{
		[SerializeField] private SpriteRenderer _spriteRenderer;
		public CircleVisualDisappearanceInfo DisappearanceInfo { get; private set; }

		public void Initialization(CircleConfigurationData configurationData)
		{
			_spriteRenderer.sprite = configurationData.Sprite;
			DisappearanceInfo = configurationData.DisappearanceInfo;
		}
	}
}
