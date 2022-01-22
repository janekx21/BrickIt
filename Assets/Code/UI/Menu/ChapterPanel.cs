using System;
using System.Linq;
using LevelContext;
using UnityEngine;
using UnityEngine.UI;
using Util;

namespace UI.Menu {
    public class ChapterPanel : MonoBehaviour {
        [SerializeField] private Text chapterName = null;
        [SerializeField] private Image image = null;
        [SerializeField] private Button button = null;
        private ChapterObject ownChapterObject;

        public void Init(ChapterObject chapterObject, Menu.OnChapterAction loadAction) {
            ownChapterObject = chapterObject;
            chapterName.text = chapterObject.chapterName;
            image.sprite = chapterObject.image;
            if (chapterObject.IsChapterUnlocked()) {
                button.onClick.AddListener(() => loadAction.Invoke(chapterObject));
            }
        }
    }
}