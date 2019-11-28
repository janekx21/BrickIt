using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Death : Block
{
    
	public override void Hit(IActor maker) {
		maker.Die();
	}
	protected override bool shouldBreak() => false;
}
