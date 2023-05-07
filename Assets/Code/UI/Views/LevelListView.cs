using System;
using LevelContext;
using UI.Menu;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Util;

namespace UI.Views {
    public class LevelListView : View {
        [SerializeField] private Menu.Menu menu;
        [SerializeField] private Button backButton;
        [SerializeField] private View chapterView;
        [SerializeField] private LevelList levelList;
        [SerializeField] private SaveScrollPosition scroll;

        private ChapterObject currentChapter;

        private void Awake() {
            backButton.onClick.AddListener(() => {
                menu.ChangeState(chapterView);
                // todo load last chapter
                // using var saveData = SaveData.Load();
                // saveData.selectedChapter = null;
                scroll.Save(currentChapter.id);
                using var data = SaveData.Load();
                data.lastChapterPlayed = "invalid chapter id";
            });
        }

        public void Init(ChapterObject chapter) {
            currentChapter = chapter;
            OnEnable();
        }

        public void OnEnable() {
            if (!currentChapter) return; // not loaded yet
            levelList.Render(currentChapter.levels);
            scroll.Load(currentChapter.id);
        }
    }
}
