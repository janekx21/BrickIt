using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using GamePlay;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace UI {
    public class Highscore : MonoBehaviour {
        [SerializeField] private Button retry = null;
        [SerializeField] private Button menu = null;
        [SerializeField] private GameObject entryContainer = null;
        [SerializeField] private GameObject entryTemplate = null;
        [SerializeField] private float templateHeight = 21f;

        private Highscores highscores;
        private List<Transform> highscoreEntryTransformList = null;
        private string key = "highscoreTable";

        public static Highscore Own => instance;
        private static Highscore instance = null;

        public UnityEvent onScoreAdded = new UnityEvent();

        private void Awake() {
            Assert.IsNull(instance);
            instance = this;
            
            retry.onClick.AddListener(() => { LevelContext.Level.Own.Retry(); });
            menu.onClick.AddListener(() => { LevelContext.Level.Own.ToMenu(); });
            onScoreAdded.AddListener(ShowHighscores);

            key = "highscoreTable" + SceneManager.GetActiveScene().buildIndex;

            if (!PlayerPrefs.HasKey(key)) {
                highscores = new Highscores { highscoreEntryList = new List<HighscoreEntry>() };
                
                string json = JsonUtility.ToJson(highscores);
                PlayerPrefs.SetString(key, json);
                PlayerPrefs.Save();
            }
            // PlayerPrefs.DeleteKey("highscoreTable");
            // PlayerPrefs.DeleteAll();

            // load saved Highscores
            string jsonString = PlayerPrefs.GetString(key);
            highscores = JsonUtility.FromJson<Highscores>(jsonString);
        }

        private void ShowHighscores() {
            foreach (Transform transform in entryContainer.transform) {
                Destroy(transform);
            }
            
            highscores.highscoreEntryList.Sort((entry1, entry2) => entry2.score - entry1.score);
            
            highscoreEntryTransformList = new List<Transform>();
            int i = 0;
            foreach (HighscoreEntry highscoreEntry in highscores.highscoreEntryList) {
                CreateHighscoreEntryTransform(highscoreEntry, entryContainer.transform, highscoreEntryTransformList);

                if (++i >= 10) {
                    break;
                }
            }
        }

        private void CreateHighscoreEntryTransform(HighscoreEntry highscoreEntry, Transform container,
            List<Transform> transformList) {
            // instantiate entry below last one
            Transform entryTransform = Instantiate(entryTemplate.transform, container.transform);
            RectTransform entryRectTransform = entryTransform.GetComponent<RectTransform>();
            entryRectTransform.anchoredPosition = new Vector2(0, -templateHeight * transformList.Count);

            // write data in HighscoreEntry
            int pos = transformList.Count + 1;
            entryTransform.Find("positionVar").GetComponent<Text>().text = pos + ".";

            int score = highscoreEntry.score;
            entryTransform.Find("scoreVar").GetComponent<Text>().text = $"{score:### ### ###}";

            string name = highscoreEntry.name;
            entryTransform.Find("nameVar").GetComponent<Text>().text = name;
            
            transformList.Add(entryTransform);
        }

        public void AddHighscoreEntry(int score, string name) {
            // create HighscoreEntry
            HighscoreEntry highscoreEntry = new HighscoreEntry { score = score, name = name };

            // add new entry to Highscores
            highscores.highscoreEntryList.Add(highscoreEntry);
            
            // save updated Highscores
            string json = JsonUtility.ToJson(highscores);
            PlayerPrefs.SetString(key, json);
            PlayerPrefs.Save();

            onScoreAdded?.Invoke();
        }

        private class Highscores {
            public List<HighscoreEntry> highscoreEntryList;
        }
        
        [Serializable]
        private class HighscoreEntry {
            public int score;
            public string name;
        }
    }
}