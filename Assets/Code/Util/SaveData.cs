using System;
using System.Collections.Generic;
using LevelContext;
using UnityEngine;

namespace Util {
    [Serializable]
    public class SaveData : IDisposable {
        [Serializable]
        public class KeyValuePair<K, V> {
            public K key;
            public V value;
        }
        
        public List<KeyValuePair<string, float>> scrollPositions = new();
        public List<string> done = new(); // id's
        public string lastMenuView; // id
        public string lastChapterPlayed; // id
        public int version = 2; // TODO make a migration structure

        private const string saveDataKey = "save_data";
        private static SaveData cache;

        public void Save() {
            // no need to re-update the cache, because there is only one save data at a time
            var json = JsonUtility.ToJson(this, true);
            PlayerPrefs.SetString(saveDataKey, json);
            PlayerPrefs.Save();
        }

        public static SaveData Load() {
            if (cache == null) {
                var json = PlayerPrefs.GetString(saveDataKey, string.Empty);
                cache = json == string.Empty ? new SaveData() : JsonUtility.FromJson<SaveData>(json);
            }
            return cache;
        }

        public void Dispose() {
            Save();
            Debug.Log("Saved: " + JsonUtility.ToJson(this, true));
        }
    }
}
