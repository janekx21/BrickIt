using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DirectionChanger : Block {
	public override void Hit(IActor actor) {
		base.Hit(actor);

		actor.ChangeDirection();
	}
	protected override bool shouldBreak() => false;
}