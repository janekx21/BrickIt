using UnityEngine;

namespace Util {
    public class AutoDestroy : MonoBehaviour {
        [SerializeField] private float delay = 1;

        private void Awake() {
            Destroy(gameObject, delay);
        }
    }
}