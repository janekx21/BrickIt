using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Block {
	public class Death : Block {
		public override void Hit(IActor maker) {
			if (ColorsMatch(maker)) {
				maker.Die();
			}
			
		}

		protected override bool shouldBreak() => false;
	}
}