using System;
using System.Diagnostics;
using UnityEngine;
using UnityEngine.Serialization;

namespace Graphics {
    
    [ExecuteInEditMode]
    [RequireComponent(typeof(Camera))]
    public class ViewportCopy : MonoBehaviour {
        [SerializeField] private GameObject parentCamera = null;

        private Camera parentCam;
        private Camera cam;

        void Awake() {
            cam = GetComponent<Camera>();
            parentCam = parentCamera.GetComponent<Camera>();
            LateUpdate();
        }

        void LateUpdate() {
            cam.orthographicSize = parentCam.orthographicSize;
            cam.rect = parentCam.rect;
        }
    }
}