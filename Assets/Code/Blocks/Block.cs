using System;
using System.Collections.Generic;
using GamePlay;
using LevelContext;
using UnityEditor;
using UnityEditor.Experimental.SceneManagement;
using UnityEngine;
using UnityEngine.Events;

namespace Blocks {
    [RequireComponent(typeof(SpriteRenderer), typeof(Rigidbody2D))]
    public abstract class Block : Entity, IColored {
        // [SerializeField] Color color = Color.white;
        [SerializeField] private ColorType colorType = ColorType.DefaultColor;

        protected Rigidbody2D rig = null;
        protected SpriteRenderer ren = null;
        protected BoxCollider2D boxCollider = null;

        private static readonly List<Block> allBlocks = new List<Block>();

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
            ren.color = ColorConversion.GetColorFromType(colorType);
            boxCollider = GetComponent<BoxCollider2D>();
        }

        public virtual void OnValidate() {
            // checks if editor is in scene or on prefab stage
            PrefabStage prefabStage = PrefabStageUtility.GetCurrentPrefabStage();
            bool isValidPrefabState = prefabStage != null && prefabStage.stageHandle.IsValid();
            bool prefabConnected = PrefabUtility.GetPrefabInstanceStatus(gameObject) ==
                                   PrefabInstanceStatus.Connected;
            if (!isValidPrefabState && prefabConnected) {
                SetSpriteColor();
            }
        }

        public override void Update() {
            base.Update();
            // ren.color = colorType.Color;
        }

        // private void OnDrawGizmos() {
        //     Gizmos.color = color;
        //     for (float i = .8f; i < 1f; i += .01f) {
        //         Gizmos.DrawWireCube(transform.position, new Vector3(1, 1, 0) * i);
        //     }
        // }

        public virtual void Hit(IActor actor) {
        }

        public virtual void Enter(IActor actor) {
        }

        public virtual void Exit(IActor actor) {
        }

        public virtual void Break(IActor maker) {
            allBlocks.Remove(this);
            if (allBlocks.TrueForAll(x => !x.shouldBreak())) {
                // i am the last Block :(
                Level.Own.Win();
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
                Enter(actor);
            }
        }

        private void OnTriggerExit2D(Collider2D other) {
            var actor = other.gameObject.GetComponent<IActor>();

            if (actor != null) {
                Exit(actor);
            }
        }

        public ColorType GetColorType() {
            return colorType;
        }

        public void SetColorType(ColorType colorType) {
            this.colorType = colorType;
            SetSpriteColor();
        }

        private void SetSpriteColor() {
            ren = GetComponent<SpriteRenderer>();
            ren.color = ColorConversion.GetColorFromType(colorType);
        }

        public bool ColorsMatch(IActor actor) {
            return GetColorType() == actor.GetColorType() || GetColorType() == ColorType.DefaultColor;
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