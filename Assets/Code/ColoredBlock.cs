using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColoredBlock : BlockColor {

    public override void Update() {
		base.Update();

        ren.color = GetColor();
    }

	public override void Hit(IActor actor) {
		base.Hit(actor);

		if (actor.GetColor() == GetColor()) {
			Break();
		}
	}
	protected override bool shouldBreak() => true;
}
