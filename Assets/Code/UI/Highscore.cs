using System.Collections.Generic;
using System.Linq;
using LevelContext;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.Events;
using UnityEngine.UI;
using Util;

namespace UI {
    public class Highscore : MonoBehaviour {
        [SerializeField] private Button retry;
        [SerializeField] private Button menu;
        [SerializeField] private GameObject entryContainer;
        [SerializeField] private GameObject entryTemplate;
        [SerializeField] private float templateHeight = 21f;

        private List<Transform> highscoreEntryTransformList;

        public static Highscore own { get; private set; }
        public UnityEvent onScoreAdded = new();

        private void Awake() {
            Assert.IsNull(own);
            own = this;
            
            retry.onClick.AddListener(() => Level.own.Retry());
            menu.onClick.AddListener(() => { Level.own.ToMenu(); });
            onScoreAdded.AddListener(ShowHighscores);
        }

        private void ShowHighscores() {
            foreach (Transform t in entryContainer.transform) {
                Destroy(t);
            }

            using var handle = SaveData.GetHandle();
            var highscores = handle.save.highScores.Where(x => x.levelGuid == Level.own.LevelId).OrderBy(x => x.score);
            
            highscoreEntryTransformList = new List<Transform>();
            var i = 0;
            foreach (var highscoreEntry in highscores) {
                CreateHighscoreEntryTransform(highscoreEntry, entryContainer.transform, highscoreEntryTransformList);

                if (++i >= 10) {
                    break;
                }
            }
        }

        private void CreateHighscoreEntryTransform(Model.V3.HighscoreEntry highscoreEntry, Transform container,
            List<Transform> transformList) {
            // instantiate entry below last one
            var entryTransform = Instantiate(entryTemplate.transform, container.transform);
            var entryRectTransform = entryTransform.GetComponent<RectTransform>();
            entryRectTransform.anchoredPosition = new Vector2(0, -templateHeight * transformList.Count);

            // write data in HighscoreEntry
            var pos = transformList.Count + 1;
            entryTransform.Find("positionVar").GetComponent<Text>().text = pos + ".";

            var score = highscoreEntry.score;
            entryTransform.Find("scoreVar").GetComponent<Text>().text = $"{score:### ### ###}";

            entryTransform.Find("nameVar").GetComponent<Text>().text = highscoreEntry.name;
            
            transformList.Add(entryTransform);
        }

        public void AddHighscoreEntry(int score, string name) {
            var highscoreEntry = new Model.V3.HighscoreEntry { score = score, name = name, levelGuid = Level.own.LevelId};

            using var handle = SaveData.GetHandle();
            handle.save.highScores.Add(highscoreEntry);
            
            onScoreAdded?.Invoke();
        }

        
    }
}
