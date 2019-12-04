using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColorChanger : Block {

	[SerializeField] private Color color = Color.white;

	public override void Update() {
		base.Update();

		ren.color = color;
	}
	
	private void OnDrawGizmos() {
		Gizmos.color = color;
		for (float i = .8f; i < 1f; i+=.01f) {
			Gizmos.DrawWireCube(transform.position, new Vector3(1,1,0) * i);
		}
	}
	
	public override void Hit(IActor actor) {
		base.Hit(actor);

		actor.SetColor(color);
	}

	protected override bool shouldBreak() => false;
}
