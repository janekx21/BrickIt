using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NormalBlock : Block {

	public override void Hit(IActor maker) {
		base.Hit(maker);

		Break();
	}

	protected override bool shouldBreak() => true;
}