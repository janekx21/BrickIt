using GamePlay;
using UnityEngine;
using UnityEngine.Events;

namespace Blocks {
	public class DirectionChanger : Block, IInteractable {
        [SerializeField] UnityEvent onInteract = new UnityEvent();
		public override void Hit(IActor actor) {
			base.Hit(actor);

			actor.ChangeDirection();
		}

		protected override bool shouldBreak() => false;
        
        public void Interact(IActor _) => onInteract.Invoke();
    }
}