using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.Assertions;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace Level {
    [CreateAssetMenu(fileName = "new ChapterContainerObject", menuName = "ChapterContainerObject", order = 0)]
    public class ChapterContainerObject : ScriptableObject {
        public ChapterObject[] chapters = new ChapterObject[0];

#if UNITY_EDITOR
        private string directory {
            get {
                var path = AssetDatabase.GetAssetPath(this);
                return Path.GetDirectoryName(path);
            }
        }

        [ContextMenu("Find All Chapter")]
        private void FindAllChapterObjects() {
            List<ChapterObject> chapterList = new List<ChapterObject>();

            var allChapterPaths = AssetDatabase.FindAssets("t:ChapterObject", new[] {directory});
            foreach (var guid in allChapterPaths) {
                var chapterPath = AssetDatabase.GUIDToAssetPath(guid);
                var lo = AssetDatabase.LoadAssetAtPath<ChapterObject>(chapterPath);
                chapterList.Add(lo);
            }

            chapters = chapterList.ToArray();
        }

        [ContextMenu("Make Chapter")]
        public void MakeChapter() {
            var guid = AssetDatabase.CreateFolder(directory, "Chapter01");
            var path = AssetDatabase.GUIDToAssetPath(guid);

            Assert.IsNotNull(path);

            var chapterName = Path.GetFileName(path);

            var scenePath = Path.Combine(path, $"{chapterName}.unity");
            var worked = AssetDatabase.CopyAsset("Assets/Scenes/ChapterMenu.unity", scenePath);

            Assert.IsTrue(worked);

            var obj = CreateInstance<ChapterObject>();
            AssetDatabase.CreateAsset(obj, Path.Combine(path, $"{chapterName}.asset"));

            FindAllChapterObjects();
        }
#endif
    }
}