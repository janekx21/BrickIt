using System;
using LevelContext;
using UnityEngine;

namespace UI {
    public class Tutorial : MonoBehaviour {
        private void Update() {
            var enabled = LevelContext.Level.Own.State == LevelState.Begin;
            foreach (Transform t in transform) {
                t.gameObject.SetActive(enabled);
            }
        }
    }
}