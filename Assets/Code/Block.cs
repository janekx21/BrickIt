using System;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer), typeof(Rigidbody2D))]
public abstract class Block : Entity {

	protected Rigidbody2D rig = null;
	protected SpriteRenderer ren = null;
	
	static List<Block> allBlocks = new List<Block>();

	private void OnEnable() {
		allBlocks.Add(this);
	}

	private void OnDisable() {
		allBlocks.Remove(this);
	}

	public override void Awake() {
		base.Awake();

		rig = GetComponent<Rigidbody2D>();
		ren = GetComponent<SpriteRenderer>();
	}

	public virtual void Hit(IActor actor) {

	}
	

	public virtual void Break() {
		if (allBlocks.Count <= 1) { // i am the last Block :(
			// TODO win the game
		}
		Destroy(gameObject);
	}

	private void OnCollisionEnter2D(Collision2D other) {
		var actor = other.gameObject.GetComponent<IActor>();

        if (actor != null) {
            Hit(actor);
        }
	}
}