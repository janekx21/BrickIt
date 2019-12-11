using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Block {
	public class Normal : Block {
		public override void Hit(IActor maker) {
			base.Hit(maker);
			if (ColorsMatch(maker)) {
				Break();
			}
		}

		protected override bool shouldBreak() => true;
	}
}