using System;
using System.Collections.Generic;
using System.Linq;
using LevelContext;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace UI.Menu {
    public class ChapterList : MonoBehaviour {
        [SerializeField] private ChapterPanel prefab = null;

        [Header("Navigation")]
        [SerializeField] private GameObject horizontalScrollbar = null;
        private Selectable lastPanelButton = null;
        private Selectable beforeLastPanelButton = null;

        private Selectable firstPanel = null;
        
        public void MarkFirstChapter() {
            firstPanel.Select();
            firstPanel.OnSelect(null);
        }

        public void Init(ChapterObject[] chapterObjects, Menu.OnChapterAction loadAction) {
            foreach (Transform t in transform) {
                Destroy(t.gameObject);
            }

            List<ChapterObject> list = chapterObjects.ToList();
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
                    Navigation navigation = new Navigation {
                        mode = Navigation.Mode.Explicit,
                        selectOnDown = horizontalScrollbar.GetComponent<Scrollbar>(),
                        selectOnRight = panel.GetComponent<Button>()
                    };
                    lastPanelButton.navigation = navigation;

                    beforeLastPanelButton = lastPanelButton;
                    lastPanelButton = panel.GetComponent<Button>();
                }
                else {
                    Navigation navigation = new Navigation {
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
                    Navigation navigation = new Navigation {
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