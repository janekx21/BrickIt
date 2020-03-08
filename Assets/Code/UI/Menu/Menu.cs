using Level;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

namespace UI.Menu {
    public class Menu : MonoBehaviour {
        enum State {
            Chapter,
            Level
        }


        [SerializeField] private ChapterContainerObject chapterContainerObject = null;
        [SerializeField] private ChapterList chapterList = null;
        [SerializeField] private LevelList levelList = null;
        [SerializeField] private Animator animator = null;

        private State currentState = State.Chapter;
        private static readonly int ChapterHash = Animator.StringToHash("chapter");
        private static readonly int LevelHash = Animator.StringToHash("level");

        public class OnChapterAction : UnityEvent<ChapterObject> {
        }

        public class OnLevelAction : UnityEvent<LevelObject> {
        }

        private void Awake() {
            var changeEvent = new OnChapterAction();
            changeEvent.AddListener(LoadChapter);
            chapterList.Init(chapterContainerObject.chapters, changeEvent);
        }

        private void Update() {
            if (Input.GetButtonDown("Cancel") && currentState == State.Level) {
                animator.SetTrigger(ChapterHash);
                currentState = State.Chapter;
            }
        }

        private void LoadChapter(ChapterObject chapter) {
            var action = new OnLevelAction();
            action.AddListener(LoadLevel);
            levelList.Init(chapter.levels, action);
            animator.SetTrigger(LevelHash);
            currentState = State.Level;
        }

        private void LoadLevel(LevelObject level) {
            SceneManager.LoadScene(level.scene);
        }
    }
}