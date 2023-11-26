using GamePlay;

namespace Blocks {
    public class Normal : Brick {
        public override void Hit(IActor maker) {
            base.Hit(maker);

            if (!ColorsMatch(maker)) return;
            
            maker.ComboAction();
            Break(maker);
        }
    }
}
