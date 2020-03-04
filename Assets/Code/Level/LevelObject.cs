using UnityEngine;
using Util;

namespace Level {
    [CreateAssetMenu]
    public class LevelObject : ScriptableObject {
        public SceneReference scene = null;
        public string levelName = "no name";
        public string levelAuthor = "no one";
        [Range(1, 20)]
        public int difficulty = 1;
    }
}
