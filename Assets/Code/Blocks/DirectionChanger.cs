using GamePlay;
using UnityEngine;
using UnityEngine.Events;

namespace Blocks {
    public class DirectionChanger : Block, IDirectionChanger, IInteractable {
        [SerializeField] UnityEvent onInteract = new UnityEvent();

        public override void Hit(IActor actor) {
            base.Hit(actor);
            if (ColorsMatch(actor)) {
                actor.ChangeDirection(this);
            }
        }

        public Vector2 GetPosition() {
            return transform.position;
        }

        protected override bool shouldBreak() => false;

        public void Interact(IActor _) => onInteract.Invoke();
    }
}