using GamePlay;
using UnityEngine;
using UnityEngine.Events;
using Util;

namespace Blocks {
    public class ColorChanger : Block, IIntractable {
        [SerializeField] UnityEvent onInteract = new UnityEvent();
        [SerializeField] private GameObject particleEffect = null;

        public override void Hit(IActor actor) {
            base.Hit(actor);

            actor.SetColor(GetColor());
        }

        protected override bool shouldBreak() => false;

        public void Interact(IActor actor) {
            if (actor.GetColor() != GetColor()) {
                onInteract.Invoke();
                var dir = actor.GetPosition() - (Vector2) transform.position;
                var effectPosition = dir.Rotation() * particleEffect.transform.localPosition;
                var clone = Instantiate(particleEffect, transform.position + effectPosition,
                    dir.Rotation());
                var system = clone.GetComponent<ParticleSystem>();
                var systemMain = system.main;
                systemMain.startColor = GetColor();
            }
        }
    }
}