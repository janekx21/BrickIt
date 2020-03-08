using Level;
using UnityEngine;

namespace Util {
    [System.Serializable]
    public class SaveData {
        public ChapterObject selectedChapter = null;
        public LevelObject selectedLevel = null;
        public float levelScrollPosition = 0;

        public void Save() {
            PlayerPrefs.SetString("saveData", JsonUtility.ToJson(this));
            PlayerPrefs.Save();
        }

        public static SaveData Load() {
            var json = PlayerPrefs.GetString("saveData", "");
            if (json == "") {
                return new SaveData();
            }
            else {
                return JsonUtility.FromJson<SaveData>(json);
            }
        }
    }
}