using Level;
using UnityEngine;
using UnityEngine.UI;

namespace UI.Menu {
	public class LevelPanel : MonoBehaviour {
		[SerializeField] private Text levelName = null;
		[SerializeField] private Text levelAuthor = null;
		[SerializeField] private Button button = null;

		public void Init(LevelObject levelObject, ChapterMenu.OnLevelAction loadAction) {
			levelName.text = levelObject.levelName;
			levelAuthor.text = levelObject.levelAuthor;
			button.onClick.AddListener(() => loadAction.Invoke(levelObject));
		}
	}
}