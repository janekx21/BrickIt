using System.Collections.Generic;
using System.Linq;
using LevelContext;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;
using Util;

namespace UI.Menu {
    public class LevelList : MonoBehaviour {
        [SerializeField] private LevelPanel prefab;

        [FormerlySerializedAs("BackButton")] [Header("Navigation")] [SerializeField]
        private GameObject backButton;

        [SerializeField] private GameObject horizontalScrollbar;
        private Selectable lastPanelButton;
        private Selectable beforeLastPanelButton;


        public void Render(IEnumerable<LevelObject> levelObjects) {
            Reset();

            var levelList = levelObjects.ToList();
            levelList.Sort(CompareOnDifficulty);
            using var saveData = SaveData.GetHandle();
            foreach (var (level, levelIndex) in levelList.Select((v, i) => (v, i))) {
                var done = saveData.save.done.Contains(level.id);
                var doneCount = levelList.Count(x => saveData.save.done.Contains(x.id));

                // add one for buffer
                var locked = !(doneCount + 1 >= levelIndex);

                var panel = Instantiate(prefab, transform);
                panel.Init(level, locked, done);

                FixNavigation(level, levelList, panel);
            }
        }

        private void Reset() {
            foreach (Transform child in transform) {
                Destroy(child.gameObject);
            }
        }

        private void FixNavigation(LevelObject level, List<LevelObject> levelList, LevelPanel panel) {
            if (level == levelList.First()) {
                MarkFirstLevel(panel.GetComponent<Button>());
                lastPanelButton = panel.GetComponent<Button>();
            }
            else if (level == levelList[1]) {
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

            if (level == levelList.Last()) {
                var navigation = new Navigation {
                    mode = Navigation.Mode.Explicit,
                    selectOnUp = backButton.GetComponent<Button>(),
                    selectOnDown = horizontalScrollbar.GetComponent<Scrollbar>(),
                    selectOnLeft = beforeLastPanelButton
                };
                panel.GetComponent<Button>().navigation = navigation;
            }
        }

        private static void MarkFirstLevel(Selectable firstPanel) {
            firstPanel.Select();
            firstPanel.OnSelect(null);
        }

        private static int CompareOnDifficulty(LevelObject x, LevelObject y) {
            return x.difficulty - y.difficulty;
        }
        

    }
}
