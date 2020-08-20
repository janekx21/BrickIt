using System;
using Blocks;
using UnityEngine;
using Util;

namespace GamePlay {
    [RequireComponent(typeof(Rigidbody2D), typeof(SpriteRenderer), typeof(AudioSource))]
    public class Player : Entity, IActor, IPausable {
        [SerializeField] private float speed = 0f;
        [SerializeField] private float moveSpeed = 1f;
        [SerializeField] private Color color = Color.red;

        [SerializeField] private float dashAcceleration = 1;
        [SerializeField] private float dashVelocity = 1;

        [SerializeField] private GameObject bounceParticle = null;

        private Rigidbody2D rig = null;
        private SpriteRenderer rend = null;
        private AudioSource bounceSource = null;
        [SerializeField] private AudioSource smallBounceSource = null;
        private Vector2 move = Vector2.zero;
        private Vector2 direction = Vector2.right;
        private float speedModifier = 1;
        private Vector2 dash = Vector2.zero;
        private bool paused = false;
        private bool directionChanged = false;


        private int combo = 0;
        private float comboTimer = 0;

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
            bounceSource = GetComponent<AudioSource>();
        }

        public override void Update() {
            base.Update();

#if DEBUG
            if (Input.GetKeyDown(KeyCode.Space)) {
                direction = new Vector2(direction.y, direction.x);
            }

            if (Input.GetKeyDown(KeyCode.P)) {
                direction *= -1;
            }
#endif
            if (comboTimer <= 0 && combo > 0) {
                ComboEnds();
            }

            comboTimer = Mathf.Clamp01(comboTimer - Time.deltaTime);
            rend.color = color;
        }

        private void ComboEnds() {
            Level.Level.Own.ApplyCombo(combo);
            combo = 0;
        }

        public override void FixedUpdate() {
            base.FixedUpdate();

            ApplyControls();
            dash = Vector2.MoveTowards(dash, Vector2.zero, Time.fixedDeltaTime * dashAcceleration);

            if (paused) {
                rig.velocity = Vector2.zero;
            }
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
                    bounceSource.PlayRandomPitch(.1f);
                }
                else {
                    smallBounceSource.PlayRandomPitch(.25f);
                }

                var block = contact.collider.GetComponent<Block>();
                var particles = Instantiate(bounceParticle, contact.point,
                    Quaternion.LookRotation(Vector3.forward, contact.normal));
                if (block) {
                    var main = particles.GetComponent<ParticleSystem>().main;
                    main.startColor = block.GetColor();
                    var interactable = block.GetComponent<IInteractable>();
                    interactable?.Interact(this);
                }

                Dash(contact.normal);
            }
        }


        private void ApplyControls() {
            if (Input.touchCount > 0 && !directionChanged) {
                Touch touch = Input.GetTouch(0);

                if (touch.position.x > Screen.width >> 1) {
                    move = Vector2.one;
                }

                else {
                    move = -Vector2.one;
                }
            }

            else if (Input.touchCount == 0 && directionChanged) {
                directionChanged = false;
            }

            else {
                move.x = Input.GetAxis("Horizontal");
                move.y = Input.GetAxis("Vertical");
            }

            Vector2 abs = new Vector2(Mathf.Abs(direction.x), Mathf.Abs(direction.y));
            Vector2 pep = Vector2.one - abs;
            Vector2 adding = pep * move;

            rig.velocity = speedModifier * speed * direction + adding * moveSpeed + dash * dashVelocity;
        }

        public void Init(Vector2 direction, Color color) {
            this.direction = direction;
            this.color = color;
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

        public void ChangeDirection(IDirectionChanger directionChanger) {
            Vector2 positionVector = (Vector2) transform.position - directionChanger.GetPosition();

            float angle = Vector2.SignedAngle(positionVector, Vector2.up);
            float roundedAngle = Mathf.Round(angle / 90f) * 90f;

            float directionalAngle = Vector2.SignedAngle(-direction, positionVector);
            float orthogonalAngle = (directionalAngle > 0 ? 1 : -1) * 90f;

            if (Vector2.SignedAngle(direction, Vector2.up) == roundedAngle) {
                direction = Quaternion.AngleAxis(orthogonalAngle, Vector3.back) * direction;
            }
            else {
                direction = Quaternion.AngleAxis(roundedAngle, Vector3.back) * Vector2.up;
            }

            directionChanged = true;
        }

        public void FlipDirection() {
            direction *= -1;
        }

        public void Die() {
            Destroy(gameObject);
            Level.Level.Own.Lose();
        }

        public Vector2 GetDirection() => direction;
        public Vector2 GetPosition() => transform.position;

        public void TeleportTo(Block @from, Block to, Vector2 dir) {
            var circleCollider2D = GetComponent<CircleCollider2D>();
            transform.position = (Vector2) to.transform.position
                                 + dir.normalized * .5f
                                 + dir.normalized * circleCollider2D.radius;
            direction = dir;
        }

        public void Dash(Vector2 direction) {
            dash = direction.normalized;
            // generate a small offset so that
            // the collision is not triggered twice
            rig.position += direction * .01f;
            Handheld.Vibrate();
        }

        /**
         * Gets called when some positive action takes place that keeps the combo alive
         */
        public void Combo() {
            combo++;
            comboTimer = 1f;
        }

        public void play() {
            paused = false;
        }

        public void pause() {
            paused = true;
        }

        public bool isPaused() => paused;
    }
}