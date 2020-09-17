using System;
using System.Collections.Generic;
using GamePlay;
using Level;
using UnityEngine;
using UnityEngine.Events;

namespace Blocks {
    [RequireComponent(typeof(SpriteRenderer), typeof(Rigidbody2D))]
    public abstract class Block : Entity, IColored {
        [SerializeField] private Color color = Color.white;

        protected Rigidbody2D rig = null;
        protected SpriteRenderer ren = null;

        private static readonly List<Block> allBlocks = new List<Block>();

        public static Color defaultColor => Color.white;
        public UnityEvent onDestroy = new UnityEvent();

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

        public virtual void OnValidate() {
            ren = GetComponent<SpriteRenderer>();
        }

        public override void Update() {
            base.Update();
            ren.color = color;
        }

        private void OnDrawGizmos() {
            Gizmos.color = color;
            for (float i = .8f; i < 1f; i += .01f) {
                Gizmos.DrawWireCube(transform.position, new Vector3(1, 1, 0) * i);
            }
        }

        public virtual void Hit(IActor actor) {
        }

        public virtual void Over(IActor actor) {
        }

        public virtual void Break(IActor maker) {
            allBlocks.Remove(this);
            if (allBlocks.TrueForAll(x => !x.shouldBreak())) {
                // i am the last Block :(
                Level.Level.Own.Win();
                maker.ComboEnds();
            }

            onDestroy.Invoke();
            Destroy(gameObject);
        }

        private void OnCollisionEnter2D(Collision2D other) {
            var actor = other.gameObject.GetComponent<IActor>();

            if (actor != null) {
                Hit(actor);
            }
        }

        private void OnTriggerEnter2D(Collider2D other) {
            var actor = other.gameObject.GetComponent<IActor>();

            if (actor != null) {
                Over(actor);
            }
        }

        public Color GetColor() {
            return color;
        }

        public void SetColor(Color color) {
            this.color = color;
        }

        public bool ColorsMatch(IActor actor) {
            return GetColor() == actor.GetColor() || GetColor() == defaultColor;
        }

        protected abstract bool shouldBreak(); // returns if the block should break to win

#if UNITY_EDITOR
        public Overview.OverviewObject ToOverviewObject() {
            var renderer = GetComponent<SpriteRenderer>();
            return new Overview.OverviewObject(renderer.sprite, Vector2Int.FloorToInt(transform.localPosition));
        }
#endif
    }
}