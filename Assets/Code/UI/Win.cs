using LevelContext;
using UnityEngine;
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
                Level.own.ChangeState(LevelState.highscores);
                Highscore.own.AddHighscoreEntry(playerScore, playerName);
                
                // save lastPlayerName
                PlayerPrefs.SetString("lastPlayerName", playerName);
                PlayerPrefs.Save();
            });
            inputName.onEndEdit.AddListener(text => {
                playerName = text.ToUpper();
                if (playerName == "") {
                    playerName = lastPlayerName;
                }
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
            time.text = $"{Level.own.timeSinceStart:000.00}";
            maxCombo.text = $"{Level.own.maxCombo:00}";
            playerScore = Level.own.score;
            score.text = $"{playerScore:### ### ###}";
        }
    }
}
