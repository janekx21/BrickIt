using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;

namespace Level {
    [CreateAssetMenu(fileName = "new ChapterObject", menuName = "ChapterObject", order = 0)]
    public class ChapterObject : ScriptableObject {
        public Sprite image = null;
        public LevelObject[] levels = new LevelObject[0];

		[ContextMenu("Find All Levels")]
		private void FindAllLevelObjects() {
            List<LevelObject> levelList = new List<LevelObject>();
            var path = AssetDatabase.GetAssetPath(this);
            var dir = Path.GetDirectoryName(path);
            
			var allLevelPaths = AssetDatabase.FindAssets("t:LevelObject", new []{dir});
			foreach (var guid in allLevelPaths) {
				var levelPath = AssetDatabase.GUIDToAssetPath(guid);
				var lo = AssetDatabase.LoadAssetAtPath<LevelObject>(levelPath);
				levelList.Add(lo);
			}

            levels = levelList.ToArray();
        }
    }
}