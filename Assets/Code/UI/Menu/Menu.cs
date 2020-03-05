using Level;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

namespace UI.Menu {
	public class Menu : MonoBehaviour {
		[SerializeField] private ChapterContainerObject chapterContainerObject = null;
		[SerializeField] private ChapterList chapterList = null;

        public class OnChapterAction : UnityEvent<ChapterObject>{}
        
		private void Awake() {
            var changeEvent = new OnChapterAction();
            changeEvent.AddListener(LoadChapter);
			chapterList.Init(chapterContainerObject.chapters , changeEvent);
		}

		private void LoadChapter(ChapterObject chapter) {
			SceneManager.LoadScene(chapter.scene.ScenePath);
		}
	}
}