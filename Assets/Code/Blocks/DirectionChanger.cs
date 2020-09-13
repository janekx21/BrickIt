using System.Security.AccessControl;
using GamePlay;
using UnityEngine;
using UnityEngine.Events;

namespace Blocks {
    
    public enum Direction {
        Left, Right
    }
    
    public class DirectionChanger : Block, IDirectionChanger, IInteractable {
        [SerializeField] UnityEvent onInteract = new UnityEvent();
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

        public void Interact(IActor _) => onInteract.Invoke();
    }
}