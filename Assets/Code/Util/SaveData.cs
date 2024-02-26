using Model;
using UnityEngine;

namespace Util {
    public class SaveData : System.IDisposable {
        public Model.V3.Save save;
        private static SaveData cache;
        private const string saveDataKey = "save_data";

        public SaveData(Model.V3.Save save) {
            this.save = save;
        }

        public void Dispose() {
            Save();
            Debug.Log("Saved: " + JsonUtility.ToJson(this, true));
        }

        public void Save() {
            // no need to re-update the cache, because there is only one save data at a time
            // TODO debounce
            var json = JsonUtility.ToJson(save, true);
            PlayerPrefs.SetString(saveDataKey, json);
            // PlayerPrefs.Save(); // Unity saves this automatically
        }

        /**
         * Gets the cached save data or reads it from disk
         */
        public static SaveData GetHandle() {
            if (cache is not null) return cache;
            
            cache = new SaveData(PlayerPrefs.GetString(saveDataKey, string.Empty) switch {
                "" =>
                    Migrator.init(),
                var json =>
                    JsonUtility.FromJson<VersionedData>(json).version switch {
                        "3" => JsonUtility.FromJson<Model.V3.Save>(json),
                        _ => Migrator.init(),
                    }
            });

            return cache;
        }
    }
}
