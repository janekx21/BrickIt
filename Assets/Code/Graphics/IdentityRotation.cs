using UnityEngine;

namespace Graphics {
    public class IdentityRotation : MonoBehaviour {
        private void LateUpdate() {
            transform.rotation = Quaternion.identity;
        }
    }
}