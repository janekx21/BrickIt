using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MultiHitBlock : Block {

	[SerializeField] private int maxHp = 2;
	private int hp = -1;

	[SerializeField] private Sprite[] sprites = null;

	public override void Awake() {
		base.Awake();
		hp = maxHp;
		Debug.Assert(sprites.Length == maxHp);
		ren.sprite = sprites[0];
	}

	public override void Hit(IActor maker) {
		base.Hit(maker);
		hp--;
		if (hp <= 0) {
			Break();
		}
		else {
			ren.sprite = sprites[maxHp-hp];
		}
	}
	protected override bool shouldBreak() => true;
}