using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColoredBlock : Block {

	[SerializeField] private Color color = Color.white;

	public override void Update() {
		base.Update();

		ren.color = color;
	}

	public override void Hit(IActor actor) {
		base.Hit(actor);

		if (actor.GetColor() == color) {
			Break();
		}
	}
}
