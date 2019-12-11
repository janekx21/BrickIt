using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Block {
	public class Spawner : Block {

        [SerializeField] private GameObject instanz = null;

        public override void Awake() {
            base.Awake();

            Instantiate(instanz, transform.position, Quaternion.identity);
        }

        protected override bool shouldBreak() => false;
	}
}