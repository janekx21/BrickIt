using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace UI {
    public class Win : MonoBehaviour {
        [SerializeField] private Button next = null;
        [SerializeField] private Text time = null;
        [SerializeField] private Text score = null;

        private void Awake() {
            next.onClick.AddListener(() => {
                // do shit
                LevelContext.Level.Own.ToMenu();
            });
        }

        private void Update() {
            time.text = $"{LevelContext.Level.Own.TimeSinceStart:000.00}";
            score.text = $"{LevelContext.Level.Own.Score:000}";
        }
    }
}