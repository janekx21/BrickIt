using UnityEngine;
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
        [ContextMenu("Generate Overview")]
        void GenerateOverview() {
            Overview.Generate(this);
        }
#endif
    }
}