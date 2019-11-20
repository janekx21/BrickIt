using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColoredBlock : Block {
	[SerializeField] private Color color = Color.white;

	public override void Update() {
		base.Update();
		ren.color = color;
	}

	public override void Hit(IActor maker) {
		base.Hit(maker);
		if (maker.GetColor() == color) {
			Break();
		}
	}
}
