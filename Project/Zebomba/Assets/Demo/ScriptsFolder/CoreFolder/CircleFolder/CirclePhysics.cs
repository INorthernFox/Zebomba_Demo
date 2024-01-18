using UnityEngine;
using Random = UnityEngine.Random;

namespace Demo.ScriptsFolder.CoreFolder.CircleFolder
{
	[RequireComponent(typeof(CircleCollider2D), typeof(Rigidbody2D))]
	public  class CirclePhysics : MonoBehaviour
	{
		[SerializeField] private CircleCollider2D _collider;
		[SerializeField] private Rigidbody2D _rigidbody2D;

		private float _forceMultiplier => _rigidbody2D.mass * 40;
		public float Speed => _rigidbody2D.velocity.magnitude;

		public void DisablePhysics()
		{
			_rigidbody2D.bodyType = RigidbodyType2D.Static;
		}
		
		public void EnablePhysics()
		{
			_rigidbody2D.bodyType = RigidbodyType2D.Dynamic;
		}

		public void AddForce(Vector2 motionVector)
		{
			Vector2 force = motionVector * _forceMultiplier;
			_rigidbody2D.AddForce(force, ForceMode2D.Impulse);
		}

		public void ResetPhysics()
		{
			_collider.enabled = true;
			_rigidbody2D.gravityScale = 1;
		}

		public void SetRandomGravity() =>
			_rigidbody2D.gravityScale = Random.Range(-1, 1.1f);
	}
}