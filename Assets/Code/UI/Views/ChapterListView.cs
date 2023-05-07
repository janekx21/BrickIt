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
            
            foreach (var chapter in chapterContainerObject.chapters) {
                if (chapter.id == SaveData.Load().lastChapterPlayed) {
                    LoadChapter(chapter);
                }
            }
        }


        private void LoadChapter(ChapterObject chapter) {
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
