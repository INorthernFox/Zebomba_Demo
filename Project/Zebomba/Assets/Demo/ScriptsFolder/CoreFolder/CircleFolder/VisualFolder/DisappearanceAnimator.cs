using System;
using System.Collections.Generic;
using Demo.ScriptsFolder.DataFolder.CirclesDataFolder;
using DG.Tweening;
using UnityEngine;

namespace Demo.ScriptsFolder.CoreFolder.CircleFolder.VisualFolder
{
	public class DisappearanceAnimator
	{
		private Dictionary<CircleObject, AnimationCase> _animations = new Dictionary<CircleObject, AnimationCase>();

		public void Animate(CircleObject circleObject, Action<CircleObject> onCompleted)
		{
			if(_animations.ContainsKey(circleObject))
				throw new ArgumentException("The circle is already animated");
			
			Transform target = circleObject.transform;
			ParticleDisappearance particleDisappearance = circleObject.ParticleDisappearance;
			CircleVisualDisappearanceInfo disappearanceInfo = circleObject.CircleVisual.DisappearanceInfo;
			
			particleDisappearance.ResetParticle();
			particleDisappearance.SetPosition(target.position);
			particleDisappearance.SetColor(disappearanceInfo.ParticleColor);
			
			particleDisappearance.transform.SetParent(null);
			
			particleDisappearance.Play();

			circleObject.OnReset += OnReset;
			List<Tweener> tweeners = new List<Tweener>(2);
			tweeners.Add(target.DORotate(target.eulerAngles + disappearanceInfo.Angle, disappearanceInfo.Time).SetEase(Ease.Linear));
			tweeners.Add(target.DOScale(Vector3.zero, disappearanceInfo.Time).SetEase(Ease.Linear).OnComplete(() => OnnComplete(circleObject, onCompleted)));
			_animations.Add(circleObject, new AnimationCase(tweeners));
		}

		private void OnReset(CircleObject circleObject)
		{
			circleObject.OnReset -= OnReset;
			
			if(_animations.ContainsKey(circleObject))
			{
				_animations[circleObject].Kill();
				_animations.Remove(circleObject);
			}
		}

		private void OnnComplete(CircleObject circleObject, Action<CircleObject> onCompleted)
		{
			circleObject.OnReset -= OnReset;
			
			Transform target = circleObject.transform;
			ParticleDisappearance particleDisappearance = circleObject.ParticleDisappearance;
			
			circleObject.gameObject.SetActive(false);

			target.localScale = Vector3.one;
			target.localRotation = Quaternion.Euler(Vector3.zero);
			
			particleDisappearance.transform.SetParent(target);
			particleDisappearance.transform.localPosition = Vector3.zero;
			particleDisappearance.ResetParticle();
			
			_animations.Remove(circleObject);
			
			onCompleted?.Invoke(circleObject);
		}
		
		private struct AnimationCase
		{
			public readonly List<Tweener> Tweeners;

			public AnimationCase(List<Tweener> tweeners) =>
				Tweeners = tweeners;

			public void Kill()
			{
				foreach(Tweener tweener in Tweeners)
					tweener.Kill();
			}
		}
	}
}