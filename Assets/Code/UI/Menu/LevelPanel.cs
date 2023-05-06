using LevelContext;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.Events;
using UnityEngine.UI;

namespace UI.Menu {
    public class LevelPanel : MonoBehaviour {
        [SerializeField] private Text levelName;
        [SerializeField] private Text levelAuthor;
        [SerializeField] private Button button;
        [SerializeField] private Image image;
        [SerializeField] private GameObject @lock;
        [SerializeField] private CanvasGroup group;

        public void Init(LevelObject levelObject, bool locked, bool done, UnityAction<LevelObject> onLoad) {
            levelName.text = levelObject.levelName;
            levelAuthor.text = levelObject.levelAuthor;
            Assert.IsNotNull(levelObject.overview, $"level named {levelObject.levelName} has no overview?");
            var sprite = Sprite.Create(levelObject.overview,
                new Rect(0f, 0f, levelObject.overview.width, levelObject.overview.height), Vector2.one * .5f);
            image.sprite = sprite;
            button.onClick.AddListener(() => onLoad(levelObject));

            @lock.SetActive(locked);
            group.interactable = !locked;
            image.color = done ? Color.white : image.color;
        }
    }
}
