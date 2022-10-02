using System;
using System.Collections.Generic;
using LevelContext;
using UnityEngine;

namespace Util {
    [Serializable]
    public class SaveData : IDisposable {
        public ChapterObject selectedChapter = null;
        public float levelScrollPosition = 0;
        public List<string> done = new(); // list of done level id's (guid)

        private const string saveDataKey = "save_data";

        public void Save() {
            var json = JsonUtility.ToJson(this, true);
            Debug.Log($"saved the following: {json}");
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