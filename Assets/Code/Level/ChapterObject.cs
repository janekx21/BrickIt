using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.Assertions;
using Util;
#if UNITY_EDITOR
using UnityEditor;

#endif

namespace Level {
    [CreateAssetMenu(fileName = "new ChapterObject", menuName = "ChapterObject", order = 0)]
    public class ChapterObject : ScriptableObject {
        public Sprite image = null;
        public LevelObject[] levels = new LevelObject[0];

#if UNITY_EDITOR
        private string directory {
            get {
                var path = AssetDatabase.GetAssetPath(this);
                return Path.GetDirectoryName(path);
            }
        }

        [ContextMenu("Find All Levels")]
        private void FindAllLevelObjects() {
            List<LevelObject> levelList = new List<LevelObject>();

            var allLevelPaths = AssetDatabase.FindAssets("t:LevelObject", new[] {directory});
            foreach (var guid in allLevelPaths) {
                var levelPath = AssetDatabase.GUIDToAssetPath(guid);
                var lo = AssetDatabase.LoadAssetAtPath<LevelObject>(levelPath);
                levelList.Add(lo);
            }

            levels = levelList.ToArray();
        }

        [ContextMenu("Make Level")]
        public void MakeLevel() {
            var guid = AssetDatabase.CreateFolder(directory, "Level01");
            var path = AssetDatabase.GUIDToAssetPath(guid);

            Assert.IsNotNull(path);

            var levelName = Path.GetFileName(path);

            var scenePath = Path.Combine(path, $"{levelName}.unity");
            var worked = AssetDatabase.CopyAsset("Assets/Scenes/Empty.unity", scenePath);

            Assert.IsTrue(worked);

            var obj = CreateInstance<LevelObject>();
            obj.scene = new SceneReference {ScenePath = scenePath};
            AssetDatabase.CreateAsset(obj, Path.Combine(path, $"{levelName}.asset"));

            FindAllLevelObjects();
        }
#endif
    }
}