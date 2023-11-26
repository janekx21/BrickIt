using GamePlay;

namespace Blocks {
    public class FlyThrough : Brick, IInteractable {
        public override void Enter(IActor actor) {
            base.Enter(actor);

            if (ColorsMatch(actor)) {
                actor.ComboAction();
                Break(actor);
            }
            else {
                boxCollider.isTrigger = false;
            }
        }

        public void Interact(IActor actor) {
            boxCollider.isTrigger = true;
        }
    }
}