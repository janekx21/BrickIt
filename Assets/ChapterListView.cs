using LevelContext;
using UI.Menu;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Util;

public class ChapterListView : MonoBehaviour {
    [SerializeField] private Menu menu;
    [SerializeField] private Button backButton;
    [SerializeField] private GameObject mainView;
    [SerializeField] private LevelListView levelView;
    [SerializeField] private ChapterList chapterList;
    [SerializeField] private ChapterContainerObject chapterContainerObject;

    private void Awake() {
        backButton.onClick.AddListener(() => menu.ChangeState(mainView));
        chapterList.Init(chapterContainerObject.chapters, LoadChapter);
    }


    private void LoadChapter(ChapterObject chapter) {
        levelView.Init(chapter);
        menu.ChangeState(levelView.gameObject);
        // todo unused. load last chapter
        //using var saveData = SaveData.Load();
        //saveData.selectedChapter = chapter;
    }
}
