using System.Collections;
using GamePlay;
using LevelContext;
using UnityEngine;

namespace Blocks {
    public class Spawner : Block {
        [SerializeField] private Player prefab;
        [SerializeField] private float waitingTimer = 1f;
        private bool ready;

        public override void Start() {
            base.Start();
            StartCoroutine(ReadyRoutine());
        }

        public void Spawn() {
            StartCoroutine(SpawnRoutine());
        }

        private IEnumerator SpawnRoutine() {
            yield return new WaitUntil(() => ready);
            var player = Instantiate(prefab, transform.position, Quaternion.identity);
            player.Init(transform.up, GetColorType());
        }

        private IEnumerator ReadyRoutine() {
            yield return new WaitForSeconds(waitingTimer);
            Level.own.ready = true;
            ready = true;
        }

        protected override bool ShouldBreak() => false;
    }
}