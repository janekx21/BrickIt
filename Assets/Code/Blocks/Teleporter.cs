using System.Linq;
using GamePlay;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.Events;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;

namespace Blocks {
    public class Teleporter : Block, IInteractable {
        [SerializeField] private AudioSource source;
        [FormerlySerializedAs("onTeleport")] public UnityEvent onTeleportFrom = new();
        public UnityEvent onTeleportTo = new();
        
        public override void Enter(IActor actor) {
            base.Enter(actor);

            if (ColorsMatch(actor)) {
                var target = FindObjectsOfType<Teleporter>()
                    .Where(teleporter => teleporter.GetColorType() == GetColorType() && teleporter != this)
                    .OrderBy(_ => Random.Range(0f, 1f))
                    .First();

                Assert.IsNotNull(target, "You need to place at least two teleporter");
                actor.TeleportTo(this, target, target.transform.up);
                onTeleportFrom.Invoke();
                target.onTeleportTo.Invoke();
                source.Play();
            }
            else {
                boxCollider.isTrigger = false;
            }
        }

        protected override bool ShouldBreak() => false;
        
        public void Interact(IActor actor) {
            boxCollider.isTrigger = true;
        }
    }
}
