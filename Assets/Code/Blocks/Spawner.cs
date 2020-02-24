using UnityEngine;
using UnityEngine.Serialization;

namespace Blocks {
	public class Spawner : Block {

        [SerializeField] private GameObject prefab = null;

        public override void Awake() {
            base.Awake();

            Instantiate(prefab, transform.position, Quaternion.identity);
        }

        protected override bool shouldBreak() => false;
	}
}