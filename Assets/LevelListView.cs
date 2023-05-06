using System.Collections;
using System.Collections.Generic;
using LevelContext;
using UI.Menu;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Util;

public class LevelListView : MonoBehaviour {
    [SerializeField] private Menu menu;
    [SerializeField] private Button backButton;
    [SerializeField] private GameObject chapterView;
    [SerializeField] private LevelList levelList;
    [SerializeField] private SaveScrollPosition scroll;

    private ChapterObject currentChapter;

    private void Awake() {
        backButton.onClick.AddListener(() => {
            menu.ChangeState(chapterView);
            // todo load last chapter
            // using var saveData = SaveData.Load();
            // saveData.selectedChapter = null;
            scroll.Save(currentChapter.GetInstanceID());
        });
    }

    public void Init(ChapterObject chapter) {
        levelList.Init(chapter.levels, LoadLevel);
        scroll.Load(chapter.GetInstanceID());
        currentChapter = chapter;
    }

    private static void LoadLevel(LevelObject levelObject) {
        var routine = SceneManager.LoadSceneAsync(levelObject.scene);
        routine.completed += _ => {
            var level = FindObjectOfType<Level>();
            level.Init(levelObject);
        };
    }
}
