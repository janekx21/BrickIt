using LevelContext;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Util;

namespace UI.Menu {
    public class Menu : MonoBehaviour {
        private enum State {
            chapter,
            level
        }


        [SerializeField] private Button backButton;
        [SerializeField] private ChapterContainerObject chapterContainerObject;
        [SerializeField] private ChapterList chapterList;
        [SerializeField] private LevelList levelList;
        [SerializeField] private Animator animator;

        [SerializeField] private ScrollRect levelScrollRect;

        private State currentState = State.chapter;
        private bool cancelIsDown;

        private static readonly int ChapterHash = Animator.StringToHash("chapter");
        private static readonly int LevelHash = Animator.StringToHash("level");

        private SaveData saveData;

        public class OnChapterAction : UnityEvent<ChapterObject> { }

        public class OnLevelAction : UnityEvent<LevelObject> { }

        private void Awake() {
            var changeEvent = new OnChapterAction();
            changeEvent.AddListener(LoadChapter);
            chapterList.Init(chapterContainerObject.chapters, changeEvent);
            backButton.onClick.AddListener(Back);
        }

        private void Start() {
            saveData = SaveData.Load();
            if (saveData.selectedChapter != null) {
                LoadChapter(saveData.selectedChapter);
                levelScrollRect.horizontalNormalizedPosition = saveData.levelScrollPosition;
            }
        }

        private void Update() {
            if (Input.GetAxisRaw("Cancel") != 0) {
                if (!cancelIsDown) {
                    Back();

                    cancelIsDown = true;
                }
            }
            else {
                cancelIsDown = false;
            }

            if (currentState == State.level) {
                saveData.levelScrollPosition = levelScrollRect.horizontalNormalizedPosition;
            }
        }

        private void Back() {
            if (currentState == State.level) {
                animator.SetTrigger(ChapterHash);
                currentState = State.chapter;
                saveData.selectedChapter = null;
                saveData.Save();
                chapterList.MarkFirstChapter();
            }
            else if (currentState == State.chapter) {
                SceneManager.LoadScene("Scenes/StartScreen");
            }
        }

        private void LoadChapter(ChapterObject chapter) {
            var action = new OnLevelAction();
            action.AddListener(LoadLevel);
            levelList.Init(chapter.levels, action);
            animator.SetTrigger(LevelHash);
            currentState = State.level;
            saveData.selectedChapter = chapter;
            saveData.Save();
            // reset scrolling
            levelScrollRect.horizontalNormalizedPosition = 0f;
        }

        private void LoadLevel(LevelObject levelObject) {
            saveData.Save();
            var routine = SceneManager.LoadSceneAsync(levelObject.scene);
            routine.completed += _ => {
                var level = FindObjectOfType<Level>();
                level.Init(levelObject);
            };
        }
    }
}
