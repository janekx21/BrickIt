using System.Collections;
using GamePlay;
using UnityEngine;

namespace Blocks {
	public class Spawner : Block {

        [SerializeField] private Player prefab = null;
        [SerializeField] private float waitingTimer = 1f;
        private bool ready = false;

        public override void Start() {
            base.Start();
            StartCoroutine(ReadyRoutine());
        }

        public void Spawn() {
            StartCoroutine(SpawnRoutine());
        }

        IEnumerator SpawnRoutine() {
            yield return new WaitUntil(() => ready);
            var player = Instantiate(prefab, transform.position, Quaternion.identity);
            player.Init(transform.up, GetColor());
        }
        
        IEnumerator ReadyRoutine() {
            yield return new WaitForSeconds(waitingTimer);
            ready = true;
        }

        protected override bool shouldBreak() => false;
	}
}