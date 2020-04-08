using System;
using Level;
using UnityEngine;

namespace UI {
    public class Tutorial : MonoBehaviour {
        private void Update() {
            bool enabled = Level.Level.Own.State == LevelState.begin;
            foreach (Transform t in transform) {
                t.gameObject.SetActive(enabled);
            }
        }
    }
}