using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D), typeof(SpriteRenderer))]
public class Player : Entity, IActor {

    [SerializeField]
    private float speed = 0f;
    [SerializeField]
    private float moveSpeed = 1f;
    [SerializeField]
    private Color color = Color.red;

    [SerializeField] private AnimationCurve dashCurve = null;

    private Rigidbody2D rig = null;
    private SpriteRenderer rend = null;
    private Vector2 move = Vector2.zero;
    private Vector2 direction = Vector2.right;
    private float speedModifier = 1;
    private Coroutine dashRoutine = null;

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
    }

    private void OnCollisionEnter2D(Collision2D other) {
        tryBounce(other);
    }

    private void OnCollisionStay2D(Collision2D other) {
        tryBounce(other);
    }

    private void tryBounce(Collision2D other) {
        foreach (var contact in other.contacts) {
            if (Vector2.Dot(contact.normal, direction) <= -.5f) {
                FlipDirection();
            }
        }
    }

    private void ApplyControls() {
        move.x = Input.GetAxis("Horizontal");
        move.y = Input.GetAxis("Vertical");

        
        Vector2 abs = new Vector2(Mathf.Abs(direction.x), Mathf.Abs(direction.y));
        Vector2 adding = (Vector2.one - abs) * move;

        rig.velocity = speedModifier * speed * direction + adding * moveSpeed;
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
        Dash();
    }

    public void Dash() {
        if (dashRoutine != null) {
           StopCoroutine(dashRoutine); 
        }
        dashRoutine = StartCoroutine(DashRoutine());
    }

    public void Die() {
        Destroy(gameObject);
    }

    IEnumerator DashRoutine() {
        float timer = 0;
        float length = dashCurve.keys.Last().time;
        while (timer < 1) {
            speedModifier = dashCurve.Evaluate(timer);
            timer += Time.deltaTime / length;
            yield return null;
        }

        speedModifier = 1;
        dashRoutine = null;
    }
}
