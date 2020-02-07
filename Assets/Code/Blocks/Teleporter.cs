using System;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Block {
	public class Teleporter : Block {
		[SerializeField] private AudioSource source = null;

		public override void Over(IActor actor) {
			base.Hit(actor);
			var target = FindObjectsOfType<Teleporter>()
				.Where(teleporter => teleporter.GetColor() == GetColor() && teleporter != this)
				.OrderBy(teleporter => Random.Range(0f,1f))
				.First();
			
			actor.TeleportTo(this, target, target.transform.up);
			source.Play();
		}

		protected override bool shouldBreak() => false;
	}
}