using System.Linq;
using LevelContext;
using UI.Menu;
using UnityEngine;
using UnityEngine.UI;
using Util;

namespace UI.Views {
    public class ChapterListView : View {
        [SerializeField] private Menu.Menu menu;
        [SerializeField] private Button backButton;
        [SerializeField] private View mainView;
        [SerializeField] private LevelListView levelView;
        [SerializeField] private ChapterList chapterList;
        [SerializeField] private ChapterContainerObject chapterContainerObject;

        private void Awake() {
            backButton.onClick.AddListener(() => menu.ChangeState(mainView));
            chapterList.Render(chapterContainerObject.chapters, LoadChapter);

            var data = SaveData.Load();
            foreach (var chapter in chapterContainerObject.chapters) {
                if (chapter.id == data.lastChapterPlayed) {
                    LoadChapter(chapter);
                }
            }
        }


        private void LoadChapter(ChapterObject chapter) {
            Debug.Log($"load chapter {chapter.name} {chapter.id}");
            levelView.Init(chapter);
            menu.ChangeState(levelView);
            using var data = SaveData.Load();
            data.lastChapterPlayed = chapter.id;
            // todo unused. load last chapter
            //using var saveData = SaveData.Load();
            //saveData.selectedChapter = chapter;
        }
    }
}
