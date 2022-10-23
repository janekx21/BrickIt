using LevelContext;
using UnityEngine;

namespace UI {
    public class Tutorial : MonoBehaviour {
        private void Update() {
            var enabled = LevelContext.Level.own.State == LevelState.begin;
            foreach (Transform t in transform) {
                t.gameObject.SetActive(enabled);
            }
        }
    }
}