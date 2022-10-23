using System.Linq;
using LevelContext;
using UnityEngine;
using UnityEngine.UI;

namespace UI.Menu {
    public class ChapterList : MonoBehaviour {
        [SerializeField] private ChapterPanel prefab;

        [Header("Navigation")]
        [SerializeField] private GameObject horizontalScrollbar;
        private Selectable lastPanelButton;
        private Selectable beforeLastPanelButton;

        private Selectable firstPanel;

        public void MarkFirstChapter() {
            firstPanel.Select();
            firstPanel.OnSelect(null);
        }

        public void Init(ChapterObject[] chapterObjects, Menu.OnChapterAction loadAction) {
            foreach (Transform t in transform) {
                Destroy(t.gameObject);
            }

            var list = chapterObjects.ToList();
//			list.Sort(Comparison);
            foreach (var o in list) {
                var panel = Instantiate(prefab, transform);
                panel.Init(o, loadAction);

                // Navigation
                if (o == list.First()) {
                    firstPanel = panel.GetComponent<Button>();
                    MarkFirstChapter();

                    lastPanelButton = panel.GetComponent<Button>();
                }
                else if (o == list[1]) {
                    var navigation = new Navigation {
                        mode = Navigation.Mode.Explicit,
                        selectOnDown = horizontalScrollbar.GetComponent<Scrollbar>(),
                        selectOnRight = panel.GetComponent<Button>()
                    };
                    lastPanelButton.navigation = navigation;

                    beforeLastPanelButton = lastPanelButton;
                    lastPanelButton = panel.GetComponent<Button>();
                }
                else {
                    var navigation = new Navigation {
                        mode = Navigation.Mode.Explicit,
                        selectOnDown = horizontalScrollbar.GetComponent<Scrollbar>(),
                        selectOnLeft = beforeLastPanelButton,
                        selectOnRight = panel.GetComponent<Button>()
                    };
                    lastPanelButton.navigation = navigation;

                    beforeLastPanelButton = lastPanelButton;
                    lastPanelButton = panel.GetComponent<Button>();
                }

                if (o == list.Last()) {
                    var navigation = new Navigation {
                        mode = Navigation.Mode.Explicit,
                        selectOnDown = horizontalScrollbar.GetComponent<Scrollbar>(),
                        selectOnLeft = beforeLastPanelButton
                    };
                    panel.GetComponent<Button>().navigation = navigation;
                }
            }
        }

//		private int Comparison(ChapterObject x, ChapterObject y) {
//			return x.difficulty - y.difficulty;
//		}
    }
}
