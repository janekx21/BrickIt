using System;
using System.Diagnostics;
using UnityEngine;
using UnityEngine.Serialization;

namespace Graphics {
    
    [ExecuteInEditMode]
    [RequireComponent(typeof(Camera))]
    public class ViewportCopy : MonoBehaviour {
        [SerializeField] private GameObject parentCamera;

        private Camera parentCam;
        private Camera cam;

        private void Awake() {
            cam = GetComponent<Camera>();
            parentCam = parentCamera.GetComponent<Camera>();
            LateUpdate();
        }

        private void LateUpdate() {
            cam.orthographicSize = parentCam.orthographicSize;
            cam.rect = parentCam.rect;
        }
    }
}