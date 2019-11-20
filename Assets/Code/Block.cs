using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer), typeof(Rigidbody2D))]
public abstract class Block : Entity {
	private Rigidbody2D rig = null;

	public override void Awake() {
		rig = GetComponent<Rigidbody2D>();
	}

	public virtual void Hit(IDamageMaker maker) {
	}

	public virtual void Break() {
		Destroy(gameObject);
	}

	private void OnCollisionEnter2D(Collision2D other) {
		var maker = other.gameObject.GetComponent<IDamageMaker>();
		if (maker != null) {
			Hit(maker);
		}
	}
}