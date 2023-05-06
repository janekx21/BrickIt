using LevelContext;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace UI.Menu {
    public class ChapterPanel : MonoBehaviour {
        [SerializeField] private Text chapterName;
        [SerializeField] private Image image;
        [SerializeField] private Button button;

        public void Init(ChapterObject chapterObject, UnityAction<ChapterObject> onLoad) {
            chapterName.text = chapterObject.chapterName;
            image.sprite = chapterObject.image;
            button.onClick.AddListener(() => onLoad.Invoke(chapterObject));
        }
    }
}
