﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColorChanger : Block {
	[SerializeField] private Color color = Color.white;

	public override void Update() {
		base.Update();
		ren.color = color;
	}
	
	public override void Hit(IActor maker) {
		base.Hit(maker);
		maker.SetColor(color);
	}
}