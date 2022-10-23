using System;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;
using Util;

namespace LevelContext {
    [CreateAssetMenu]
    public class LevelObject : ScriptableObject {
        public string id;
        public SceneReference scene;
        public string levelName = "no name";
        public string levelAuthor = "no one";
        [Range(1, 20)] public int difficulty = 1;

        public Texture2D overview;
        public List<Model.Tile> levelData = new();

        private void OnValidate() {
#if UNITY_EDITOR
            if (string.IsNullOrEmpty(id) || !GUID.TryParse(id, out _)) {
                id = GUID.Generate().ToString();
            }
#endif
        }

        protected bool Equals(LevelObject other) {
            return base.Equals(other) && id == other.id;
        }
        public override bool Equals(object obj) {
            if (ReferenceEquals(null, obj))
                return false;
            if (ReferenceEquals(this, obj))
                return true;
            if (obj.GetType() != this.GetType())
                return false;
            return Equals((LevelObject) obj);
        }
        public override int GetHashCode() {
            return HashCode.Combine(base.GetHashCode(), id);
        }

#if UNITY_EDITOR
        [ContextMenu("Rename Level")]
        public void RenameLevel() {
            var newName = levelName.Replace('?', '-');

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
        private void GenerateOverview() {
            Overview.Generate(this);
        }
#endif
    }
}