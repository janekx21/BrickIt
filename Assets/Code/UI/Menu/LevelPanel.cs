using LevelContext;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace UI.Menu {
    public class LevelPanel : MonoBehaviour {
        [SerializeField] private Text levelName;
        [SerializeField] private Text levelAuthor;
        [SerializeField] private Button button;
        [SerializeField] private Image image;
        [SerializeField] private GameObject @lock;
        [SerializeField] private CanvasGroup group;
        [SerializeField] private GameObject check;

        public void Init(LevelObject levelObject, bool locked, bool done) {
            levelName.text = levelObject.levelName;
            levelAuthor.text = levelObject.levelAuthor;
            Assert.IsNotNull(levelObject.overview, $"level named {levelObject.levelName} has no overview?");
            var sprite = Sprite.Create(levelObject.overview,
                new Rect(0f, 0f, levelObject.overview.width, levelObject.overview.height), Vector2.one * .5f);
            image.sprite = sprite;
            button.onClick.AddListener(() => LoadLevel(levelObject));

            @lock.SetActive(locked);
            group.interactable = !locked;
            image.color = done ? Color.white : image.color;
            check.SetActive(done);
        }

        private static void LoadLevel(LevelObject levelObject) {
            var current = SceneManager.GetActiveScene();
            var routine = SceneManager.LoadSceneAsync("LoadableLevel", LoadSceneMode.Additive);
            routine.completed += _ => {
                foreach (var rootGameObject in current.GetRootGameObjects()) {
                    rootGameObject.SetActive(false);
                }
                
                SceneManager.SetActiveScene(SceneManager.GetSceneByName("LoadableLevel"));
                var level = FindObjectOfType<Level>();
                level.Init(levelObject);
            };
        }
    }
}
