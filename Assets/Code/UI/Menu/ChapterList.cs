using System.Collections.Generic;
using System.Linq;
using Level;
using UnityEngine;

namespace UI.Menu {
    public class ChapterList : MonoBehaviour {
        [SerializeField] private ChapterPanel prefab = null;

        public void Init(ChapterObject[] chapterObjects, Menu.OnChapterAction loadAction) {
            foreach (Transform t in transform) {
                Destroy(t.gameObject);
            }

            List<ChapterObject> list = chapterObjects.ToList();
//			list.Sort(Comparison);
            foreach (var o in list) {
                var panel = Instantiate(prefab, transform);
                panel.Init(o, loadAction);
            }
        }

//		private int Comparison(ChapterObject x, ChapterObject y) {
//			return x.difficulty - y.difficulty;
//		}
    }
}