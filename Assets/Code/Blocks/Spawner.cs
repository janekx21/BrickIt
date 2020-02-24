using GamePlay;
using UnityEngine;

namespace Blocks {
	public class Spawner : Block {

        [SerializeField] private Player prefab = null;

        public override void Awake() {
            base.Awake();

            var player = Instantiate(prefab, transform.position, Quaternion.identity);
            player.Init(transform.up, GetColor());
            
        }

        protected override bool shouldBreak() => false;
	}
}