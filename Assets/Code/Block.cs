using UnityEngine;

[RequireComponent(typeof(SpriteRenderer), typeof(Rigidbody2D))]
public abstract class Block : Entity {
	private Rigidbody2D rig;

	public override void Awake() {
		base.Awake();
		rig = GetComponent<Rigidbody2D>();
	}

	public virtual void Hit(IActor maker) {
	}
	

	public virtual void Break() {
		Destroy(gameObject);
	}

	private void OnCollisionEnter2D(Collision2D other) {
		var maker = other.gameObject.GetComponent<IActor>();
		if (maker != null) Hit(maker);
	}
}