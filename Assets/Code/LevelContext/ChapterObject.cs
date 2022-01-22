using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;
using UnityEngine.Assertions;
using Util;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace LevelContext {
    [CreateAssetMenu(fileName = "new ChapterObject", menuName = "ChapterObject", order = 0)]
    public class ChapterObject : ScriptableObject {
        public Sprite image = null;
        public string chapterName = "no name";
        [Range(0, 1)] private float unlockPercentage = 0.8f;
        public LevelObject[] levels = new LevelObject[0];
        
        public bool IsChapterUnlocked() {
            var chapterContainerObject = FindObjectOfType<ChapterContainerObject>();
            int i = Array.IndexOf(chapterContainerObject.chapters, this);
            
            if (i == 0) {
                return true;
            }
            
            i--;
            var previousChapter = chapterContainerObject.chapters[i];
            using SaveData data = SaveData.Load();
            int numberOfDoneLevels = previousChapter.levels.Count(levelObject => data.done[levelObject]);
            
            return numberOfDoneLevels / (float) previousChapter.levels.Length >= unlockPercentage;
        }
        
#if UNITY_EDITOR
        private string directory {
            get {
                var path = AssetDatabase.GetAssetPath(this);
                return Path.GetDirectoryName(path);
            }
        }

        [ContextMenu("Find All Levels")]
        private void FindAllLevelObjects() {
            var levelList = levels.ToList();

            var allLevelPaths = AssetDatabase.FindAssets("t:LevelObject", new[] {directory});
            foreach (var guid in allLevelPaths) {
                var levelPath = AssetDatabase.GUIDToAssetPath(guid);
                var lo = AssetDatabase.LoadAssetAtPath<LevelObject>(levelPath);
                if (!levelList.Contains(lo)) {
                    levelList.Add(lo);
                }
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
        
        [ContextMenu("Rename Chapter")]
        public void RenameChapter() {
            var newName = chapterName.Replace('?', '-');
            
            var levelObjectPath = AssetDatabase.GetAssetPath(this);
            AssetDatabase.RenameAsset(levelObjectPath, newName);

            var oldPath = Path.GetDirectoryName(levelObjectPath);
            var parentPath = Directory.GetParent(oldPath).ToString();
            var newPath = parentPath + "\\" + newName;
            AssetDatabase.MoveAsset(oldPath, newPath);
        }
#endif
    }
}