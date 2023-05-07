using System.Linq;
using UnityEditor;
using UnityEngine;

namespace Util {
    public class Id : MonoBehaviour {
        public string id;
        private void OnValidate() {
#if UNITY_EDITOR
            if (string.IsNullOrEmpty(id) || !GUID.TryParse(id, out _)) {
                id = GUID.Generate().ToString();
            }
#endif
        }

        public static Id FindById(string id) => FindObjectsOfType<Id>().FirstOrDefault(x => x.id == id);
    }
}
