using LevelContext;
using UnityEngine;

namespace UI {
    public class Tutorial : MonoBehaviour {
        private void Update() {
            var active = LevelContext.Level.own.state == LevelState.begin;
            foreach (Transform t in transform) {
                t.gameObject.SetActive(active);
            }
        }
    }
}
