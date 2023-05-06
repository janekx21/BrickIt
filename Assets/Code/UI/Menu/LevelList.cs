using System.Linq;
using LevelContext;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;
using UnityEngine.UI;
using Util;

namespace UI.Menu {
    public class LevelList : MonoBehaviour {
        [SerializeField] private LevelPanel prefab;

        [FormerlySerializedAs("BackButton")]
        [Header("Navigation")]
        [SerializeField] private GameObject backButton;
        [SerializeField] private GameObject horizontalScrollbar;
        private Selectable lastPanelButton;
        private Selectable beforeLastPanelButton;
        
        private Selectable firstPanel;

        private void MarkFirstLevel() {
            firstPanel.Select();
            firstPanel.OnSelect(null);
        }
        
        public void Init(LevelObject[] levelObjects, UnityAction<LevelObject> onLoad) {
            foreach (Transform t in transform) {
                Destroy(t.gameObject);
            }

            var list = levelObjects.ToList();
            list.Sort(CompareOnDifficulty);
            var saveData = SaveData.Load();
            foreach (var (o,i) in list.Select((v,i) => (v, i))) {
                var done = saveData.done.Contains(o);
                var doneCount = list.Count(x => saveData.done.Contains(x));

                // add one for buffer
                var locked = !(doneCount + 1 >= i);
                
                var panel = Instantiate(prefab, transform);
                panel.Init(o, locked, done, onLoad);

                // Navigation
                if (o == list.First()) {
                    firstPanel = panel.GetComponent<Button>();
                    MarkFirstLevel();
                    
                    lastPanelButton = panel.GetComponent<Button>();
                }
                else if (o == list[1]) {
                    var navigation = new Navigation {
                        mode = Navigation.Mode.Explicit,
                        selectOnUp = backButton.GetComponent<Button>(),
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
                        selectOnUp = backButton.GetComponent<Button>(),
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
                        selectOnUp = backButton.GetComponent<Button>(),
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
