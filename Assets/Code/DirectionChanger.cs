using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DirectionChanger : Block {
	public override void Hit(IActor maker) {
		base.Hit(maker);
		maker.ChangeDirection();
	}
}