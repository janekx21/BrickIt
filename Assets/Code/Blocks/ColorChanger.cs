using GamePlay;
using UnityEngine;
using UnityEngine.Events;

namespace Blocks {
	public class ColorChanger : Block, IIntractable {

        [SerializeField] UnityEvent onInteract = new UnityEvent();
		public override void Hit(IActor actor) {
			base.Hit(actor);

			actor.SetColor(GetColor());
		}

		protected override bool shouldBreak() => false;
        public void Interact() {
            onInteract.Invoke();
        }
    }

}
