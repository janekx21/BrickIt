using System.Collections.Generic;
using System.Linq;
using LevelContext;
using UnityEngine;

namespace UI.Menu {
    public class LevelList : MonoBehaviour {
        [SerializeField] private LevelPanel prefab = null;


        public void Init(LevelObject[] levelObjects, Menu.OnLevelAction loadAction) {
            foreach (Transform t in transform) {
                Destroy(t.gameObject);
            }

            List<LevelObject> list = levelObjects.ToList();
            list.Sort(Comparison);
            foreach (var o in list) {
                var panel = Instantiate(prefab, transform);
                panel.Init(o, loadAction);
            }
        }

        private int Comparison(LevelObject x, LevelObject y) {
            return x.difficulty - y.difficulty;
        }
    }
}