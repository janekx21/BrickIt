using GamePlay;
using LevelContext;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UIElements;
using Util;

namespace Blocks {
    public class SpeedChanger : Block, ISpeedChanger {
        [SerializeField] private AudioSource sourceSpeedUp;
        private Vector2 enterDirection;
            
        public override void Enter(IActor actor) {
            base.Enter(actor);
            
            CheckForActivation(actor);
            
            enterDirection = actor.GetDirection();
        }

        public override void Exit(IActor actor) {
            base.Exit(actor);
            
            if (actor.GetDirection() == -enterDirection) {
                CheckForActivation(actor);
            }
        }

        private void CheckForActivation(IActor actor) {
            if (ColorsMatch(actor)) {
                actor.ChangeSpeed(this);

                var normalizedSpeed = actor.GetNormalizedSpeed();
                if (actor.GetDirection() == GetDirection()) {
                    sourceSpeedUp.time = 0f;
                    sourceSpeedUp.PlayRandomPitch(0f, normalizedSpeed);
                }
                else if (actor.GetDirection() == -GetDirection()) {
                    sourceSpeedUp.timeSamples = sourceSpeedUp.clip.samples - 1;
                    sourceSpeedUp.PlayRandomPitch(0f, -normalizedSpeed);
                }
            }
        }

        protected override bool shouldBreak() => false;

        public Vector2 GetDirection() {
            return transform.up;
        }
    }
}