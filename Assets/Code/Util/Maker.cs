using UnityEngine;

namespace Util {
    public class Maker : MonoBehaviour, ICanMake {
        [SerializeField] private GameObject prefab = null;

        public void Spawn() {
            Spawn(prefab);
        }

        public void Spawn(GameObject obj) {
            Instantiate(obj, transform.position, transform.rotation, null);
        }
    }
}