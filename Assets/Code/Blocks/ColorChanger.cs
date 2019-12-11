using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Block {
	public class ColorChanger : Block {

		public override void Update() {
			base.Update();

			ren.color = GetColor();
		}
	
		public override void Hit(IActor actor) {
			base.Hit(actor);

			actor.SetColor(GetColor());
		}

		protected override bool shouldBreak() => false;
	}

}
