using Model;
using Newtonsoft.Json;
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
            Debug.Log("Saved: " + JsonConvert.SerializeObject(this, Formatting.Indented));
        }

        public void Save() {
            // no need to re-update the cache, because there is only one save data at a time
            // TODO debounce
            var json = JsonConvert.SerializeObject(save, Formatting.Indented);
            PlayerPrefs.SetString(saveDataKey, json);
            // PlayerPrefs.Save(); // Unity saves this automatically when exiting
        }

        /**
         * Gets the cached save data or reads it from disk
         */
        public static SaveData GetHandle() {
            if (cache is not null) return cache;
            
            cache = new SaveData(PlayerPrefs.GetString(saveDataKey, string.Empty) switch {
                "" =>
                    Migrator.Init(),
                var json =>
                    JsonConvert.DeserializeObject<VersionedData>(json).version switch {
                        "3" => JsonConvert.DeserializeObject<Model.V3.Save>(json),
                        _ => Migrator.Init(),
                    }
            });

            return cache;
        }
    }
}
