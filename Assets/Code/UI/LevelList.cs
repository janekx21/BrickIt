using System;
using UnityEngine;

namespace UI {
	public class LevelList : MonoBehaviour {
		[SerializeField] private LevelPanel prefab = null;

		private void Awake() {
		}

		public void Init(LevelObject[] levelObjects, Level.ParameterAction<LevelObject> loadAction) {
			foreach (Transform t in transform) {
				Destroy(t.gameObject);
			}
			foreach (var o in levelObjects) {
				var panel = Instantiate(prefab, transform);
				panel.Init(o, loadAction.Invoke);
			}
		}

	}
}