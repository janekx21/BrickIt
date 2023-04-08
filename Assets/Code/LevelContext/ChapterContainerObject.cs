using System;
using System.IO;
using System.Linq;
using UnityEngine;
using UnityEngine.Assertions;
#if UNITY_EDITOR
using UnityEditor;

#endif

namespace LevelContext {
    [CreateAssetMenu(fileName = "new ChapterContainerObject", menuName = "ChapterContainerObject", order = 0)]
    public class ChapterContainerObject : ScriptableObject {
        public ChapterObject[] chapters = Array.Empty<ChapterObject>();

#if UNITY_EDITOR
        private string directory {
            get {
                var path = AssetDatabase.GetAssetPath(this);
                return Path.GetDirectoryName(path);
            }
        }

        [ContextMenu("Find All Chapter")]
        private void FindAllChapterObjects() {
            var chapterList = chapters.ToList();

            var allChapterPaths = AssetDatabase.FindAssets("t:ChapterObject", new[] {directory});
            foreach (var guid in allChapterPaths) {
                var chapterPath = AssetDatabase.GUIDToAssetPath(guid);
                var lo = AssetDatabase.LoadAssetAtPath<ChapterObject>(chapterPath);
                if (!chapterList.Contains(lo)) {
                    chapterList.Add(lo);
                }
            }

            chapters = chapterList.ToArray();
        }

        [ContextMenu("Make Chapter")]
        public void MakeChapter() {
            var guid = AssetDatabase.CreateFolder(directory, "Chapter01");
            var path = AssetDatabase.GUIDToAssetPath(guid);

            Assert.IsNotNull(path);

            var chapterName = Path.GetFileName(path);

            var obj = CreateInstance<ChapterObject>();
            AssetDatabase.CreateAsset(obj, Path.Combine(path, $"{chapterName}.asset"));

            FindAllChapterObjects();
        }

        [ContextMenu("Rename All Levels")]
        public void RenameAllLevels() {
            foreach (var chapter in chapters) {
                foreach (var level in chapter.levels) {
                    level.RenameLevel();
                }
            }
        }

        [ContextMenu("Generate All Overviews")]
        public void GenerateAllOverviews() {
            foreach (var chapter in chapters) {
                foreach (var level in chapter.levels) {
                    Overview.Generate(level);
                }
            }
        }
#endif
    }
}
