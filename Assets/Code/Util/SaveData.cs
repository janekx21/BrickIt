using System;
using System.Collections.Generic;
using LevelContext;
using UnityEngine;

namespace Util {
    [Serializable]
    public class SaveData : IDisposable {
        public ChapterObject selectedChapter;
        public float levelScrollPosition;
        public List<string> done = new(); // list of done level id's (guid)

        private const string saveDataKey = "save_data";

        public void Save() {
            var json = JsonUtility.ToJson(this, true);
            PlayerPrefs.SetString(saveDataKey, json);
            PlayerPrefs.Save();
        }

        public static SaveData Load() {
            var json = PlayerPrefs.GetString(saveDataKey, String.Empty);
            return json == string.Empty ? new SaveData() : JsonUtility.FromJson<SaveData>(json);
        }

        public void Dispose() {
            Save();
        }
    }
}
