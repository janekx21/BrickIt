using System.Collections.Generic;
using Level;
using UnityEditor;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

namespace UI.Menu {
	public class Menu : MonoBehaviour {
		[SerializeField] private ChapterObject chapterObject = null;
		[SerializeField] private LevelList list = null;

        public class OnLevelAction : UnityEvent<LevelObject>{}
        
		private void Awake() {
            var changeEvent = new OnLevelAction();
            changeEvent.AddListener(LoadLevel);
			list.Init(chapterObject.levels, changeEvent);
		}

		[ContextMenu("Find All Levels")]
		private void FindAllLevelObjects() {
            List<LevelObject> levelList = new List<LevelObject>();
			var allLevelPaths = AssetDatabase.FindAssets("t:LevelObject");
			foreach (var guid in allLevelPaths) {
				var levelPath = AssetDatabase.GUIDToAssetPath(guid);
				var lo = AssetDatabase.LoadAssetAtPath<LevelObject>(levelPath);
				levelList.Add(lo);
			}

            chapterObject.levels = levelList.ToArray();
        }

		private void LoadLevel(LevelObject level) {
			SceneManager.LoadScene(level.scene.ScenePath);
		}
	}
}