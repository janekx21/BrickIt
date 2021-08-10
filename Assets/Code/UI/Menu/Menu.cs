using System.Collections.Generic;
using System.Linq;
using LevelContext;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Util;

namespace UI.Menu {
    public class Menu : MonoBehaviour {
        enum State {
            Chapter,
            Level
        }


        [SerializeField] private Button backButton = null;
        [SerializeField] private ChapterContainerObject chapterContainerObject = null;
        [SerializeField] private ChapterList chapterList = null;
        [SerializeField] private LevelList levelList = null;
        [SerializeField] private Animator animator = null;

        [SerializeField] private ScrollRect levelScrollRect = null;

        private State currentState = State.Chapter;
        private bool cancelIsDown = false;

        private static readonly int ChapterHash = Animator.StringToHash("chapter");
        private static readonly int LevelHash = Animator.StringToHash("level");

        private SaveData saveData = null;

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

            if (currentState == State.Level) {
                saveData.levelScrollPosition = levelScrollRect.horizontalNormalizedPosition;
            }
        }

        void Back() {
            if (currentState == State.Level) {
                animator.SetTrigger(ChapterHash);
                currentState = State.Chapter;
                saveData.selectedChapter = null;
                saveData.Save();
                chapterList.MarkFirstChapter();
            }
            else if (currentState == State.Chapter) {
                SceneManager.LoadScene("Scenes/StartScreen");
            }

            Debug.Log(currentState);
        }

        private void LoadChapter(ChapterObject chapter) {
            var action = new OnLevelAction();
            action.AddListener(LoadLevel);
            levelList.Init(chapter.levels, action);
            animator.SetTrigger(LevelHash);
            currentState = State.Level;
            saveData.selectedChapter = chapter;
            saveData.Save();
            // reset scrolling
            levelScrollRect.horizontalNormalizedPosition = 0f;
        }

        private void LoadLevel(LevelObject levelObject) {
            saveData.Save();
            var routine = SceneManager.LoadSceneAsync(levelObject.scene);
            routine.completed += operation => {
                var level = FindObjectOfType<Level>();
                level.Init(levelObject);
            };
        }
    }
}
