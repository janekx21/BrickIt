using System.Linq;
using GamePlay;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.Events;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;

namespace Blocks {
    public class Teleporter : Block, IInteractable {
        [SerializeField] private AudioSource source = null;
        [FormerlySerializedAs("onTeleport")] public UnityEvent onTeleportFrom = new UnityEvent();
        public UnityEvent onTeleportTo = new UnityEvent();
        private BoxCollider2D collider = null;

        private void Awake() {
            base.Awake();
            
            collider = GetComponent<BoxCollider2D>();
        }
        
        public override void Over(IActor actor) {
            base.Over(actor);

            if (ColorsMatch(actor)) {
                var target = FindObjectsOfType<Teleporter>()
                    .Where(teleporter => teleporter.GetColor() == GetColor() && teleporter != this)
                    .OrderBy(teleporter => Random.Range(0f, 1f))
                    .First();

                Assert.IsNotNull(target, "You need to place at least two teleporter");
                actor.TeleportTo(this, target, target.transform.up);
                onTeleportFrom.Invoke();
                target.onTeleportTo.Invoke();
                source.Play();
            }
            else {
                collider.isTrigger = false;
            }
        }

        protected override bool shouldBreak() => false;
        
        public void Interact(IActor actor) {
            collider.isTrigger = true;
        }
    }
}