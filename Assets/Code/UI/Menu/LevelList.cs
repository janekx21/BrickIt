using System.Collections.Generic;
using System.Linq;
using LevelContext;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace UI.Menu {
    public class LevelList : MonoBehaviour {
        [SerializeField] private LevelPanel prefab = null;

        [Header("Navigation")]
        [SerializeField] private GameObject BackButton = null;
        [SerializeField] private GameObject HorizontalScrollbar = null;
        private Selectable lastPanelButton = null;
        private Selectable beforeLastPanelButton = null;
        
        private Selectable firstPanel = null;
        
        public void MarkFirstLevel() {
            firstPanel.Select();
            firstPanel.OnSelect(null);
        }
        
        public void Init(LevelObject[] levelObjects, Menu.OnLevelAction loadAction) {
            foreach (Transform t in transform) {
                Destroy(t.gameObject);
            }

            List<LevelObject> list = levelObjects.ToList();
            list.Sort(Comparison);
            foreach (var o in list) {
                var panel = Instantiate(prefab, transform);
                panel.Init(o, loadAction);

                // Navigation
                if (o == list.First()) {
                    firstPanel = panel.GetComponent<Button>();
                    MarkFirstLevel();
                    
                    lastPanelButton = panel.GetComponent<Button>();
                }
                else if (o == list[1]) {
                    Navigation navigation = new Navigation {
                        mode = Navigation.Mode.Explicit,
                        selectOnUp = BackButton.GetComponent<Button>(),
                        selectOnDown = HorizontalScrollbar.GetComponent<Scrollbar>(),
                        selectOnRight = panel.GetComponent<Button>()
                    };
                    lastPanelButton.navigation = navigation;

                    beforeLastPanelButton = lastPanelButton;
                    lastPanelButton = panel.GetComponent<Button>();
                }
                else {
                    Navigation navigation = new Navigation {
                        mode = Navigation.Mode.Explicit,
                        selectOnUp = BackButton.GetComponent<Button>(),
                        selectOnDown = HorizontalScrollbar.GetComponent<Scrollbar>(),
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
                        selectOnUp = BackButton.GetComponent<Button>(),
                        selectOnDown = HorizontalScrollbar.GetComponent<Scrollbar>(),
                        selectOnLeft = beforeLastPanelButton
                    };
                    panel.GetComponent<Button>().navigation = navigation;
                }
            }
        }

        private int Comparison(LevelObject x, LevelObject y) {
            return x.difficulty - y.difficulty;
        }
    }
}