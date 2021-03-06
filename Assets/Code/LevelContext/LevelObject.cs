using System;
using System.IO;
using System.Linq;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
using Util;

namespace LevelContext {
    [CreateAssetMenu]
    public class LevelObject : ScriptableObject {
        public SceneReference scene = null;
        public string levelName = "no name";
        public string levelAuthor = "no one";
        [Range(1, 20)] public int difficulty = 1;

        public Texture2D overview = null;

#if UNITY_EDITOR
        [ContextMenu("Rename Level")]
        public void RenameLevel() {
            string newName = levelName.Replace('?', '-');
            
            var levelObjectPath = AssetDatabase.GetAssetPath(this);
            var overviewPath = AssetDatabase.GetAssetPath(overview);
            AssetDatabase.RenameAsset(levelObjectPath, newName);
            AssetDatabase.RenameAsset(scene.ScenePath, newName);
            AssetDatabase.RenameAsset(overviewPath, newName);

            var oldPath = Path.GetDirectoryName(levelObjectPath);
            var parentPath = Directory.GetParent(oldPath).ToString();
            var newPath = parentPath + "\\" + newName;
            AssetDatabase.MoveAsset(oldPath, newPath);
        }

        [ContextMenu("Generate Overview")]
        void GenerateOverview() {
            Overview.Generate(this);
        }
#endif
    }
}