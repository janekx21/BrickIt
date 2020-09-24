using System;
using UnityEngine;
using UnityEngine.UI;

namespace UI {
    public class LevelTimer : MonoBehaviour {
        [SerializeField] private Text text = null;

        private void Update() {
            text.text = $"{LevelContext.Level.Own.TimeSinceStart:000}";
        }
    }
}