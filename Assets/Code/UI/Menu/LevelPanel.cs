using Level;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.UI;

namespace UI.Menu {
    public class LevelPanel : MonoBehaviour {
        [SerializeField] private Text levelName = null;
        [SerializeField] private Text levelAuthor = null;
        [SerializeField] private Button button = null;
        [SerializeField] private Image image = null;

        public void Init(LevelObject levelObject, Menu.OnLevelAction loadAction) {
            levelName.text = levelObject.levelName;
            levelAuthor.text = levelObject.levelAuthor;
            Assert.IsNotNull(levelObject.overview, $"level named {levelObject.levelName} has no overview?");
            var sprite = Sprite.Create(levelObject.overview,
                new Rect(0f, 0f, levelObject.overview.width, levelObject.overview.height), Vector2.one * .5f);
            image.sprite = sprite;
            button.onClick.AddListener(() => loadAction.Invoke(levelObject));
        }
    }
}