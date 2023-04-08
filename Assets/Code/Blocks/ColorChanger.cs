using GamePlay;
using UnityEngine;
using UnityEngine.Events;
using Util;

namespace Blocks {
    public class ColorChanger : Block, IInteractable {
        [SerializeField] private UnityEvent onInteract = new();
        [SerializeField] private GameObject particleEffect;

        protected override bool ShouldBreak() => false;

        public void Interact(IActor actor) {
            if (actor.GetColorType() == GetColorType()) return; // has the color already
            actor.SetColorType(GetColorType());

            onInteract.Invoke();
            var dir = actor.GetPosition() - (Vector2) transform.position;
            var effectPosition = dir.normalized.Rotation() * particleEffect.transform.localPosition;
            var clone = Instantiate(particleEffect, transform.position + effectPosition, dir.Rotation());
            var system = clone.GetComponent<ParticleSystem>();
            var systemMain = system.main;
            systemMain.startColor = ColorConversion.Convert(GetColorType());
        }
    }
}
