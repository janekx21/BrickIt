using System;
using GamePlay;
using LevelContext;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace UI {
    public class Win : MonoBehaviour {
        [SerializeField] private Button next = null;
        [SerializeField] private Text time = null;
        [SerializeField] private Text score = null;
        [SerializeField] private Text maxCombo = null;
        [SerializeField] private InputField inputName = null;
        
        private string playerName = "";
        private string lastPlayerName = "PLAYER";
        private int playerScore = 0;

        private void Awake() {
            next.onClick.AddListener(() => {
                Level.Own.ChangeState(LevelState.Highscores);
                Highscore.Own.AddHighscoreEntry(playerScore, playerName);
                
                // save lastPlayerName
                PlayerPrefs.SetString("lastPlayerName", playerName);
                PlayerPrefs.Save();
            });
            inputName.onEndEdit.AddListener(text => {
                playerName = text.ToUpper();
                if (playerName == "") {
                    playerName = lastPlayerName;
                }
                Debug.Log(playerName);
            });
            
            // initialize lastPlayerName save
            if (!PlayerPrefs.HasKey("lastPlayerName")) {
                PlayerPrefs.SetString("lastPlayerName", "PLAYER");
                PlayerPrefs.Save();   
            }

            // load saved lastPlayerName
            lastPlayerName = PlayerPrefs.GetString("lastPlayerName");
            inputName.placeholder.GetComponent<Text>().text = lastPlayerName;
            playerName = lastPlayerName;
        }

        private void Update() {
            time.text = $"{Level.Own.TimeSinceStart:000.00}";
            maxCombo.text = $"{Level.Own.MaxCombo:00}";
            playerScore = Level.Own.Score;
            score.text = $"{playerScore:### ### ###}";
        }
    }
}