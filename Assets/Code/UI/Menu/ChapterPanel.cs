using LevelContext;
using UnityEngine;
using UnityEngine.UI;

namespace UI.Menu {
    public class ChapterPanel : MonoBehaviour {
        [SerializeField] private Text chapterName;
        [SerializeField] private Image image;
        [SerializeField] private Button button;

        public void Init(ChapterObject chapterObject, Menu.OnChapterAction loadAction) {
            chapterName.text = chapterObject.chapterName;
            image.sprite = chapterObject.image;
            button.onClick.AddListener(() => loadAction.Invoke(chapterObject));
        }
    }
}