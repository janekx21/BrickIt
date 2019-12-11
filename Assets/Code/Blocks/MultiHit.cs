using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Block {
	public class MultiHit : Brick {
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
				ren.sprite = sprites[maxHp - hp];
			}
		}
	}
}