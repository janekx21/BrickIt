using GamePlay;

namespace Blocks {
	public class ColorChanger : Block {

		public override void Hit(IActor actor) {
			base.Hit(actor);

			actor.SetColor(GetColor());
		}

		protected override bool shouldBreak() => false;
	}

}
