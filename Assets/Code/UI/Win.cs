using System;
using GamePlay;
using LevelContext;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace UI {
    public class Win : MonoBehaviour {
        [SerializeField] private Button next;
        [SerializeField] private Text time;
        [SerializeField] private Text score;
        [SerializeField] private Text maxCombo;
        [SerializeField] private InputField inputName;
        
        private string playerName = "";
        private string lastPlayerName = "PLAYER";
        private int playerScore;

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