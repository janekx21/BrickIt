using System;
using GamePlay;
using LevelContext;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace UI {
    public class Win : MonoBehaviour {
        [SerializeField] private Button next = null;
        [SerializeField] private Text time = null;
        [SerializeField] private Text score = null;
        [SerializeField] private InputField inputName = null;
        private string playername = "PLAYER";

        private void Awake() {
            next.onClick.AddListener(() => {
                Level.Own.ChangeState(LevelState.Highscores);
            });
            inputName.onEndEdit.AddListener(text => {
                playername = text.ToUpper();
                Debug.Log(playername);
            });
        }

        private void Update() {
            time.text = $"{LevelContext.Level.Own.TimeSinceStart:000.00}";
            score.text = $"{LevelContext.Level.Own.Score:000}";
        }
    }
}