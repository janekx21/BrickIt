using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Block {
	public class Normal : Brick {
		
		[SerializeField]
		Sprite[] possibleSprites = new Sprite[0];
		
		public override void Start() {
			base.Start();
			var position = transform.position;
			int index = Mathf.FloorToInt(position.x*131 + position.y * 17);
			ren.sprite = possibleSprites[index % possibleSprites.Length];
		}

		public override void Hit(IActor maker) {
			base.Hit(maker);
			if (ColorsMatch(maker)) {
				Break();
			}
		}
	}
}