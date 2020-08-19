using GamePlay;

namespace Blocks {
    public class Death : Block {
        public override void Hit(IActor maker) {
            if (ColorsMatch(maker)) {
                maker.Die();
            }
        }

        protected override bool shouldBreak() => false;
    }
}