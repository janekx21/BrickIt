using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D), typeof(SpriteRenderer))]
public class Player : Entity, IActor {
	[SerializeField] private float speed = 0f;
	[SerializeField] private float moveSpeed = 1f;
	[SerializeField] private Color color = Color.red;
	
	[SerializeField] private float dashAcceleration = 1;
	[SerializeField] private float dashVelocity = 1;

	private Rigidbody2D rig = null;
	private SpriteRenderer rend = null;
	private Vector2 move = Vector2.zero;
	private Vector2 direction = Vector2.right;
	private float speedModifier = 1;
	private Vector2 dash = Vector2.zero;
   
    private void OnDrawGizmos() {
        Gizmos.color = color;
        for (float i = .8f; i < 1f; i += .01f) {
            Gizmos.DrawWireSphere(transform.position, i * 0.3f);
        }
    }

    public override void Awake() {
		base.Awake();

		rig = GetComponent<Rigidbody2D>();
		rend = GetComponent<SpriteRenderer>();
	}

	public override void Update() {
		base.Update();

		if (Input.GetKeyDown(KeyCode.Space)) {
			direction = new Vector2(direction.y, direction.x);
		}

		if (Input.GetKeyDown(KeyCode.P)) {
			direction *= -1;
		}

		rend.color = color;
	}

	public override void FixedUpdate() {
		base.FixedUpdate();

		ApplyControls();
		dash = Vector2.MoveTowards(dash, Vector2.zero, Time.fixedDeltaTime * dashAcceleration);
	}

	private void OnCollisionEnter2D(Collision2D other) {
		TryBounce(other);
	}

	private void OnCollisionStay2D(Collision2D other) {
		TryBounce(other);
	}

	private void TryBounce(Collision2D other) {
		foreach (var contact in other.contacts) {
			if (Vector2.Dot(contact.normal, direction) <= -.5f) {
				FlipDirection();
			}
			Dash(contact.normal);
		}
	}

	private void ApplyControls() {
		move.x = Input.GetAxis("Horizontal");
		move.y = Input.GetAxis("Vertical");

		Vector2 abs = new Vector2(Mathf.Abs(direction.x), Mathf.Abs(direction.y));
		Vector2 pep = Vector2.one - abs;
		Vector2 adding = pep * move;

		rig.velocity = speedModifier * speed * direction + adding * moveSpeed + dash*dashVelocity;
	}

	public float GetDamage() {
		return 1;
	}

	public Color GetColor() {
		return color;
	}

	public void SetColor(Color color) {
		this.color = color;
	}

	public void ChangeDirection() {
		direction = new Vector2(direction.y, direction.x);
	}

	public void FlipDirection() {
		direction *= -1;
	}

	public void Die() {
		Destroy(gameObject);
	}

	public void Dash(Vector2 direction) {
		dash = direction;
	}
}