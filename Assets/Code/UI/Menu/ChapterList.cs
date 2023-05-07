using System.Collections.Generic;
using System.Linq;
using LevelContext;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using Util;

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

        public void Render(IEnumerable<ChapterObject> chapterObjects, UnityAction<ChapterObject> onLoad) {
            Reset();

            var chapters = chapterObjects.ToList();
            foreach (var chapter in chapters) {
                var panel = Instantiate(prefab, transform);
                panel.Init(chapter, onLoad);
                FixNavigation(chapter, chapters, panel);
            }
        }

        private void FixNavigation(ChapterObject chapter, List<ChapterObject> chapters, ChapterPanel panel) {
            if (chapter == chapters.First()) {
                firstPanel = panel.GetComponent<Button>();
                MarkFirstChapter();

                lastPanelButton = panel.GetComponent<Button>();
            }
            else if (chapter == chapters[1]) {
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

            if (chapter == chapters.Last()) {
                var navigation = new Navigation {
                    mode = Navigation.Mode.Explicit,
                    selectOnDown = horizontalScrollbar.GetComponent<Scrollbar>(),
                    selectOnLeft = beforeLastPanelButton
                };
                panel.GetComponent<Button>().navigation = navigation;
            }
        }

        private void Reset() {
            foreach (Transform t in transform) {
                Destroy(t.gameObject);
            }
        }
    }
}
