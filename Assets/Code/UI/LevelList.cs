using System;
using System.Collections.Generic;
using System.Linq;
using Level;
using UnityEngine;

namespace UI {
	public class LevelList : MonoBehaviour {
		[SerializeField] private LevelPanel prefab = null;

		private void Awake() {
		}

		public void Init(LevelObject[] levelObjects, Level.Level.ParameterAction<LevelObject> loadAction) {
			foreach (Transform t in transform) {
				Destroy(t.gameObject);
			}

			List<LevelObject> list = levelObjects.ToList();
			list.Sort(Comparison);
			foreach (var o in list) {
				var panel = Instantiate(prefab, transform);
				panel.Init(o, loadAction.Invoke);
			}
		}

		private int Comparison(LevelObject x, LevelObject y) {
			return x.difficulty - y.difficulty;
		}
	}
}