using System;
using System.Collections.Generic;
using Level;
using UnityEngine;

namespace Util {
    [System.Serializable]
    public class SaveData : IDisposable {
        public ChapterObject selectedChapter = null;
        public float levelScrollPosition = 0;
        public LevelObject selectedLevel = null;
        public List<LevelObject> done = new List<LevelObject>();
        
        private const string saveDataKey = "save_data";

        public void Save() {
            var json = JsonUtility.ToJson(this, true);
            PlayerPrefs.SetString(saveDataKey, json);
            PlayerPrefs.Save();
        }

        public static SaveData Load() {
            var json = PlayerPrefs.GetString(saveDataKey, String.Empty);
            if (json == String.Empty) {
                return new SaveData();
            }
            return JsonUtility.FromJson<SaveData>(json);
        }

        public void Dispose() {
            Save();
        }
    }
}