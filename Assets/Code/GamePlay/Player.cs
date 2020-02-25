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
        private Vector2 move = Vector2.zero;
        private Vector2 direction = Vector2.right;
        private float speedModifier = 1;
        private Vector2 dash = Vector2.zero;
        private bool paused = false;

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

            rend.color = color;
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
            bounceSource.PlayOverlapping();
            foreach (var contact in other.contacts) {
                if (Vector2.Dot(contact.normal, direction) <= -.5f) {
                    FlipDirection();
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
            move.x = Input.GetAxis("Horizontal");
            move.y = Input.GetAxis("Vertical");

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

        public void ChangeDirection() {
            direction = new Vector2(direction.y, direction.x);
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
                                 + dir.normalized * (circleCollider2D.radius);
            direction = dir;
        }

        public void Dash(Vector2 direction) {
            dash = direction.normalized;
            // generate a small offset so that
            // the collision is not triggered twice
            rig.position += direction * .01f;
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