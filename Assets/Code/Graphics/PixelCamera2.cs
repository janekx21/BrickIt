﻿using System;
using System.Diagnostics;
using UnityEngine;
using UnityEngine.Serialization;

namespace Graphics {
    
    [ExecuteInEditMode]
    [DisallowMultipleComponent]
    [RequireComponent(typeof(Camera))]
    public class PixelCamera2 : MonoBehaviour {
        [SerializeField] private int referenceHeight = 180;
        [SerializeField] private bool useAutomaticWidth = false;
        [SerializeField] private int referenceWidth = 320;
        [SerializeField] private int pixelsPerUnit = 32;
        [SerializeField] private bool blit = true;

        private int actualWidth;
        private int actualHeight;

        private Camera cam;

        void Awake() {
            cam = GetComponent<Camera>();
            Update();
        }

        void Update() {
            /*
            Orthographic size is half of reference resolution since it is measured
            from center to the top of the screen.
        */
            cam.orthographicSize = referenceHeight * 0.5f / pixelsPerUnit;

            if (useAutomaticWidth) {
                int scale = Screen.height / referenceHeight;

                // Height is snapped to the closest whole multiple of reference height.
                actualHeight = referenceHeight * scale;
                    
                /*
                    Width isn't snapped like height is and will fill the entire width of 
                    the monitor using the scale determined by the height.
                */
                referenceWidth = Screen.width / scale;
                actualWidth = referenceWidth * scale;
            }
            else {
                // zoom level (PPU scale)
                int verticalZoom = Screen.height / referenceHeight;
                int horizontalZoom = Screen.width / referenceWidth;
                int scale = Math.Max(1, Math.Min(verticalZoom, horizontalZoom));

                // Height and Width is snapped to the closest whole multiple of reference value.
                actualWidth = referenceWidth * scale;
                actualHeight = referenceHeight * scale;
            }

            Rect rect = cam.rect;

            rect.width = (float) actualWidth / Screen.width;
            rect.height = (float) actualHeight / Screen.height;

            rect.x = (1f - rect.width) / 2f;
            rect.y = (1f - rect.height) / 2f;

            cam.rect = rect;
        }

        void OnRenderImage(RenderTexture source, RenderTexture destination) {
            if (blit) {
                RenderTexture buffer = RenderTexture.GetTemporary(referenceWidth, referenceHeight, -1);

                buffer.filterMode = FilterMode.Point;
                source.filterMode = FilterMode.Point;
                UnityEngine.Graphics.Blit(source, buffer);
                UnityEngine.Graphics.Blit(buffer, destination);

                RenderTexture.ReleaseTemporary(buffer);
            }
            else {
                UnityEngine.Graphics.Blit(source, destination);
            }
        }

        private void OnDrawGizmos() {
            var vert = cam.orthographicSize * 2;
            var size = new Vector2(Mathf.Floor(vert * (16f / 9f)), vert);
            Gizmos.color = Color.white;
            var position = this.transform.position;
            Gizmos.DrawWireCube(position, size);
            Gizmos.color = Color.gray;
            Gizmos.DrawWireCube(position, size + Vector2.one * .1f);
        }
    }
}