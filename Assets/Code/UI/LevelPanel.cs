using UnityEngine;
using UnityEngine.UI;

namespace UI {
	public class LevelPanel : MonoBehaviour {
		[SerializeField] private Text levelName = null;
		[SerializeField] private Text levelAuthor = null;
		[SerializeField] private Button button = null;

		public void Init(LevelObject levelObject, Level.ParameterAction<LevelObject> loadAction) {
			levelName.text = levelObject.levelName;
			levelAuthor.text = levelObject.levelAuthor;
			button.onClick.AddListener(() => loadAction.Invoke(levelObject));
		}
	}
}