using System.Collections;
using System.Collections.Generic;
using System.Linq;
using LevelContext;
using Newtonsoft.Json;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.Events;
using UnityEngine.Networking;
using UnityEngine.UI;
using Util;

namespace UI {
    public class Highscore : MonoBehaviour {
        [SerializeField] private Button retry;
        [SerializeField] private Button menu;
        [SerializeField] private Transform entryContainer;
        [SerializeField] private GameObject entryTemplate;
        [SerializeField] private GameObject loadingSpinner;
        [SerializeField] private GameObject errorPopup;

        public static Highscore own { get; private set; }
        public UnityEvent onScoreAdded = new();
        public string lastUsedUserId = null;

        private void Awake() {
            Assert.IsNull(own);
            own = this;

            retry.onClick.AddListener(() => Level.own.Retry());
            menu.onClick.AddListener(() => { Level.own.ToMenu(); });
            onScoreAdded.AddListener(ShowHighscores);
        }

        private void ShowHighscores() {
            var levelGuid = Level.own.LevelId;
            StartCoroutine(FetchRoutine(levelGuid));
        }

        private void RenderHighscores(Dictionary<string, int> highscores) {
            foreach (Transform t in entryContainer) {
                Destroy(t.gameObject);
            }

            foreach (var ((userId, score), index) in highscores.OrderByDescending(x => x.Value).Take(7)
                         .Select((pair, index) => (pair, index))) {
                var entry = ViewHighscore(userId, score, index + 1);
                entry.transform.SetParent(entryContainer);
                entry.transform.localScale = Vector3.one;
            }
        }

        private GameObject ViewHighscore(string userId, int score, int place) {
            var entry = Instantiate(entryTemplate);
            var isMy = userId == lastUsedUserId;
            var rect = entry.GetComponent<RectTransform>();
            rect.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, isMy ? 40f : 30f);
            // TODO this does not work :<
            // rect.localScale = Vector3.one * (isMy ? 1.04f : 1);
            entry.transform.Find("underline").gameObject.SetActive(isMy);
            entry.transform.Find("positionVar").GetComponent<Text>().text = place + ".";
            entry.transform.Find("scoreVar").GetComponent<Text>().text = $"{score:### ### ###}";
            entry.transform.Find("nameVar").GetComponent<Text>().text = userId;
            return entry;
        }

        public void AddHighscoreEntry(int score, string name) {
            lastUsedUserId = name;
            var levelGuid = Level.own.LevelId;
            var highscoreEntry = new Model.V3.HighscoreEntry { score = score, name = name, levelGuid = levelGuid };

            using var handle = SaveData.GetHandle();
            handle.save.highScores.Add(highscoreEntry);

            var highscore = new Model.Backend.Highscore { userId = name, score = (uint)score };
            StartCoroutine(UploadRoutine(highscore, levelGuid));

            onScoreAdded?.Invoke();
        }

        public IEnumerator UploadRoutine(Model.Backend.Highscore highscore, string levelGuid) {
            var post = UnityWebRequest.Post($"https://highscore-gw01lfrs.fermyon.app/highscore/level/{levelGuid}",
                JsonConvert.SerializeObject(highscore), "application/json");

            var request = post.SendWebRequest();
            loadingSpinner.SetActive(true);
            yield return request;
            loadingSpinner.SetActive(false);
            if (request.webRequest.result != UnityWebRequest.Result.Success) {
                errorPopup.SetActive(true);
            }

            yield return FetchRoutine(levelGuid);
        }

        public IEnumerator FetchRoutine(string levelGuid) {
            var get = UnityWebRequest.Get($"https://highscore-gw01lfrs.fermyon.app/highscore/level/{levelGuid}");
            var request = get.SendWebRequest();
            loadingSpinner.SetActive(true);
            yield return request;
            loadingSpinner.SetActive(false);
            if (request.webRequest.result != UnityWebRequest.Result.Success) {
                errorPopup.SetActive(true);
            }
            else {
                var level = JsonConvert.DeserializeObject<Model.Backend.Level>(request.webRequest.downloadHandler.text);
                RenderHighscores(level.highscores);
            }
        }
    }
}
