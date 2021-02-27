using System;
using GamePlay;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace UI {
    public class Highscores : MonoBehaviour {
        [SerializeField] private Button retry = null;
        [SerializeField] private Button menu = null;
        [SerializeField] private GameObject entryContainer = null;
        [SerializeField] private GameObject entryTemplate = null;

        private void Awake() {
            retry.onClick.AddListener(() => { LevelContext.Level.Own.Retry(); });
            menu.onClick.AddListener(() => { LevelContext.Level.Own.ToMenu(); });

           float templateHeight = 21f;
           for (int i = 0; i < 10; i++) {
               Transform entryTransform = Instantiate(entryTemplate.transform, entryContainer.transform);
               RectTransform entryRectTransform = entryTransform.GetComponent<RectTransform>();
               entryRectTransform.anchoredPosition = new Vector2(0, -templateHeight * i);

               entryTransform.Find("positionVar").GetComponent<Text>().text = i + 1 + ".";
               
               entryTransform.Find("scoreVar").GetComponent<Text>().text = "999";
               entryTransform.Find("nameVar").GetComponent<Text>().text = "PLAYER";
           }
        }

        private void Update() {
            // time.text = $"{LevelContext.Level.Own.TimeSinceStart:000.00}";
            // score.text = $"{LevelContext.Level.Own.Score:000}";
        }
    }
}