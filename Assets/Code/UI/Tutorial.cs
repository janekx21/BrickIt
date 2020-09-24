using System;
using LevelContext;
using UnityEngine;

namespace UI {
    public class Tutorial : MonoBehaviour {
        private void Update() {
            bool enabled = LevelContext.Level.Own.State == LevelState.begin;
            foreach (Transform t in transform) {
                t.gameObject.SetActive(enabled);
            }
        }
    }
}