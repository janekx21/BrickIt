using System.Collections.Generic;
using GamePlay;
using LevelContext;
using UnityEditor;
#if UNITY_EDITOR

#endif
using UnityEngine;
using UnityEngine.Events;

namespace Blocks {
    /**
     * A Block is a grid based static interaction partner for the IActor.
     */
    [RequireComponent(typeof(SpriteRenderer), typeof(Rigidbody2D))]
    public abstract class Block : Entity, IColored {
        [SerializeField] private ColorType colorType = ColorType.@default;

        protected Rigidbody2D rig;
        protected SpriteRenderer ren;
        protected BoxCollider2D boxCollider;

        private static readonly List<Block> allBlocks = new();

        public UnityEvent onDestroy = new();

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
            ren.color = ColorConversion.Convert(colorType);
            boxCollider = GetComponent<BoxCollider2D>();
        }

#if UNITY_EDITOR
        public virtual void OnValidate() {
            // checks if editor is in scene or on prefab stage
            var prefabStage = UnityEditor.SceneManagement.PrefabStageUtility.GetCurrentPrefabStage();
            var isValidPrefabState = prefabStage != null && prefabStage.stageHandle.IsValid();
            var prefabConnected = PrefabUtility.GetPrefabInstanceStatus(gameObject) ==
                                  PrefabInstanceStatus.Connected;
            if (!isValidPrefabState && prefabConnected) {
                SetSpriteColor();
            }
        }
#endif

        public virtual void Hit(IActor actor) { }

        public virtual void Enter(IActor actor) { }

        public virtual void Exit(IActor actor) { }

        public virtual void Break(IActor maker) {
            allBlocks.Remove(this);
            if (allBlocks.TrueForAll(x => !x.shouldBreak())) {
                // i am the last Block :(
                Level.own.Win();
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
            ren.color = ColorConversion.Convert(colorType);
        }

        public bool ColorsMatch(IActor actor) {
            return GetColorType() == actor.GetColorType() || GetColorType() == ColorType.@default;
        }

        /**
         * returns if the block should break to win
         */
        protected abstract bool shouldBreak();

#if UNITY_EDITOR
        public Overview.OverviewObject ToOverviewObject() {
            var renderer = GetComponent<SpriteRenderer>();
            return new Overview.OverviewObject(renderer.sprite, Vector2Int.FloorToInt(transform.localPosition));
        }
#endif
    }
}
