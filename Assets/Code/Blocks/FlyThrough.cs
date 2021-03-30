using GamePlay;

namespace Blocks {
    public class FlyThrough : Brick, IInteractable {
        public override void Over(IActor actor) {
            base.Over(actor);

            if (ColorsMatch(actor)) {
                Break(actor);
                actor.Combo();
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