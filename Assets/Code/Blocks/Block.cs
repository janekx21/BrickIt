using System.Collections.Generic;
using GamePlay;
using LevelContext;
using Model;
using UnityEditor;
#if UNITY_EDITOR

#endif
using UnityEngine;
using UnityEngine.Events;
using Level = LevelContext.Level;

namespace Blocks {
    /**
     * A Block is a grid based static interaction partner for the IActor.
     */
    [RequireComponent(typeof(SpriteRenderer), typeof(Rigidbody2D))]
    public abstract class Block : Entity, IColored {
        [SerializeField] private ColorType colorType = ColorType.@default;

        protected SpriteRenderer ren;
        protected BoxCollider2D boxCollider;

        private static readonly List<Block> AllBlocks = new();

        public UnityEvent onDestroy = new();

        private void OnEnable() {
            AllBlocks.Add(this);
        }

        private void OnDisable() {
            AllBlocks.Remove(this);
        }

        public override void Awake() {
            base.Awake();

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
            AllBlocks.Remove(this);
            if (AllBlocks.TrueForAll(x => !x.ShouldBreak())) {
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

        public void SetColorType(ColorType type) {
            colorType = type;
            SetSpriteColor();
        }

        private void SetSpriteColor() {
            ren = GetComponent<SpriteRenderer>();
            ren.color = ColorConversion.Convert(colorType);
        }

        protected bool ColorsMatch(IActor actor) {
            return GetColorType() == actor.GetColorType() || GetColorType() == ColorType.@default;
        }

        /**
         * returns if the block should break to win
         */
        protected abstract bool ShouldBreak();

#if UNITY_EDITOR
        public Overview.OverviewObject ToOverviewObject() {
            var spriteRenderer = GetComponent<SpriteRenderer>();
            return new Overview.OverviewObject(spriteRenderer.sprite, Vector2Int.FloorToInt(transform.localPosition));
        }
#endif
    }
}
