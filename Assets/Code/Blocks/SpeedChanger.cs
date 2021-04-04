using GamePlay;
using UnityEngine;

namespace Blocks {
    public class SpeedChanger : Block, ISpeedChanger {
        private Vector2 enterDirection;
            
        public override void Enter(IActor actor) {
            base.Enter(actor);
            
            if (ColorsMatch(actor)) {
                actor.ChangeSpeed(this);
            }
            
            enterDirection = actor.GetDirection();
        }

        public override void Exit(IActor actor) {
            base.Exit(actor);

            if (actor.GetDirection() == -enterDirection) {
                if (ColorsMatch(actor)) {
                    actor.ChangeSpeed(this);
                }
            }
        }

        protected override bool shouldBreak() => false;

        public Vector2 GetDirection() {
            return transform.up;
        }
    }
}