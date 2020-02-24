using System.Collections.Generic;
using Level;
using UnityEditor;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

namespace UI.Menu {
	public class Menu : MonoBehaviour {
		[SerializeField] private List<LevelObject> levelObjects = new List<LevelObject>();
		[SerializeField] private LevelList list = null;

        public class OnLevelAction : UnityEvent<LevelObject>{}
        
		private void Awake() {
            var changeEvent = new OnLevelAction();
            changeEvent.AddListener(LoadLevel);
			list.Init(levelObjects.ToArray(), changeEvent);
		}

		[ContextMenu("Find All Levels")]
		private void FindAllLevelObjects() {
			levelObjects.Clear();
			var allLevelPaths = AssetDatabase.FindAssets("t:LevelObject");
			foreach (var guid in allLevelPaths) {
				var levelPath = AssetDatabase.GUIDToAssetPath(guid);
				var lo = AssetDatabase.LoadAssetAtPath<LevelObject>(levelPath);
				levelObjects.Add(lo);
			}
		}

		private void LoadLevel(LevelObject level) {
			SceneManager.LoadScene(level.scene.ScenePath);
		}
	}
}