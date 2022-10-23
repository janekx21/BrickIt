using GamePlay;
using UnityEngine;
using UnityEngine.Events;

namespace Blocks {
    
    public enum Direction {
        Left, Right
    }
    
    public class DirectionChanger : Block, IDirectionChanger, IInteractable {
        [SerializeField]
        private UnityEvent onInteract = new();
        [SerializeField] private Direction direction = Direction.Right;

        public override void Hit(IActor actor) {
            base.Hit(actor);
            if (ColorsMatch(actor)) {
                actor.ChangeDirection(this);
            }
        }

        public Vector2 GetPosition() {
            return transform.position;
        }

        public Direction GetDirection() {
            return direction;
        }

        protected override bool shouldBreak() => false;

        public void Interact(IActor actor) {
            
            // this is a redundant call, which could be handled in Hit
            if (ColorsMatch(actor)) {
                onInteract.Invoke();
            }
        }
    }
}