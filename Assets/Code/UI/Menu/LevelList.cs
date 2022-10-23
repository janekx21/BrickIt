using System.Linq;
using LevelContext;
using UnityEngine;
using UnityEngine.UI;
using Util;

namespace UI.Menu {
    public class LevelList : MonoBehaviour {
        [SerializeField] private LevelPanel prefab;

        [Header("Navigation")]
        [SerializeField] private GameObject BackButton;
        [SerializeField] private GameObject horizontalScrollbar;
        private Selectable lastPanelButton;
        private Selectable beforeLastPanelButton;
        
        private Selectable firstPanel;
        
        public void MarkFirstLevel() {
            firstPanel.Select();
            firstPanel.OnSelect(null);
        }
        
        public void Init(LevelObject[] levelObjects, Menu.OnLevelAction loadAction) {
            foreach (Transform t in transform) {
                Destroy(t.gameObject);
            }

            var list = levelObjects.ToList();
            list.Sort(CompareOnDifficulty);
            foreach (var (o,i) in list.Select((v,i) => (v, i))) {
                var panel = Instantiate(prefab, transform);

                using var saveData = SaveData.Load();
                var done = saveData.done.Contains(o.id);
                var doneCount = list.Count(x => saveData.done.Contains(x.id));

                // add one for buffer
                var locked = !(doneCount + 1 >= i);
                
                panel.Init(o, locked, done, loadAction);

                // Navigation
                if (o == list.First()) {
                    firstPanel = panel.GetComponent<Button>();
                    MarkFirstLevel();
                    
                    lastPanelButton = panel.GetComponent<Button>();
                }
                else if (o == list[1]) {
                    var navigation = new Navigation {
                        mode = Navigation.Mode.Explicit,
                        selectOnUp = BackButton.GetComponent<Button>(),
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
                        selectOnUp = BackButton.GetComponent<Button>(),
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
                        selectOnUp = BackButton.GetComponent<Button>(),
                        selectOnDown = horizontalScrollbar.GetComponent<Scrollbar>(),
                        selectOnLeft = beforeLastPanelButton
                    };
                    panel.GetComponent<Button>().navigation = navigation;
                }
            }
        }

        private static int CompareOnDifficulty(LevelObject x, LevelObject y) {
            return x.difficulty - y.difficulty;
        }
    }
}