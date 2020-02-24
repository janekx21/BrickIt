using GamePlay;

namespace Blocks {
	public class DirectionChanger : Block {
		public override void Hit(IActor actor) {
			base.Hit(actor);

			actor.ChangeDirection();
		}

		protected override bool shouldBreak() => false;
	}
}