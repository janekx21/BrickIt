using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class Player : Entity {

    [SerializeField]
    private float speed = 0f;
    [SerializeField]
    private float moveSpeed = 1f;

    private Rigidbody2D rig;
    private Vector2 move = Vector2.zero;
    private Vector2 direction = Vector2.right;

    public override void Awake() {
        base.Awake();

        rig = gameObject.GetComponent<Rigidbody2D>();
    }

    public override void Update() {
        base.Update();
        if (Input.GetKeyDown(KeyCode.Space)) {
            direction = new Vector2(direction.y, direction.x);
        }
        if (Input.GetKeyDown(KeyCode.P)) {
            direction *= -1;
        }
    }

    public override void FixedUpdate() {
        base.FixedUpdate();

        ApplyControls();
    }

    private void ApplyControls() {
        move.x = Input.GetAxis("Horizontal");
        move.y = Input.GetAxis("Vertical");

        
        Vector2 abs = new Vector2(Mathf.Abs(direction.x), Mathf.Abs(direction.y));
        Vector2 adding = (Vector2.one - abs) * move;

        rig.velocity = direction * speed + adding * moveSpeed;
    }
}
