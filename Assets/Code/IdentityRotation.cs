using System;
using UnityEngine;

public class IdentityRotation : MonoBehaviour {
    private void LateUpdate() {
        transform.rotation = Quaternion.identity;
    }
}