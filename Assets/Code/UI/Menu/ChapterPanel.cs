using Level;
using UnityEngine;
using UnityEngine.UI;

namespace UI.Menu {
	public class ChapterPanel : MonoBehaviour {
		[SerializeField] private Text chapterName = null;
		[SerializeField] private Image image = null;
		[SerializeField] private Button button = null;

		public void Init(ChapterObject chapterObject, Menu.OnChapterAction loadAction) {
			chapterName.text = chapterObject.name;
			image.sprite = chapterObject.image;
			button.onClick.AddListener(() => loadAction.Invoke(chapterObject));
		}
	}
}