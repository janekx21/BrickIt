using UnityEngine;

namespace GamePlay {
    /**
     * Entity wraps the MonoBehaviour to name Unity Events
     */
    public abstract class Entity : MonoBehaviour {
        public virtual void Awake() {
        }

        public virtual void Start() {
        }

        public virtual void Update() {
        }

        public virtual void FixedUpdate() {
        }

        public virtual void LateUpdate() {
        }
    }
}
