using UnityEngine;
using UnityEngine.UI;

namespace UI {
    public class LevelTimer : MonoBehaviour {
        [SerializeField] private Text text;

        private void Update() {
            text.text = $"{LevelContext.Level.own.TimeSinceStart:000}";
        }
    }
}