using System;
using UnityEngine;

namespace Graphics {
    [ExecuteInEditMode]
    public class PixelCamera : MonoBehaviour {
        public int referenceHeight = 180;
        public int pixelsPerUnit = 32;
        [SerializeField] private bool blit = true;

        private int renderWidth;
        private int renderHeight;
        private int actualWidth;
        private int actualHeight;

        private Camera cam;

        private void Awake() {
            cam = GetComponent<Camera>();
            Update();
        }

        private void Update() {
            /*
            Orthographic size is half of reference resolution since it is measured
            from center to the top of the screen.
        */

            renderHeight = referenceHeight;
            cam.orthographicSize = renderHeight * .5f / pixelsPerUnit;

            var scale = Screen.height / renderHeight;

            // Height is snapped to the closest whole multiple of reference height.
            actualHeight = renderHeight * scale;

            /*
            Width isn't snapped like height is and will fill the entire width of 
            the monitor using the scale determined by the height.
        */
            renderWidth = Screen.width / scale;
            actualWidth = renderWidth * scale;

            var rect = cam.rect;

            rect.width = (float) actualWidth / Screen.width;
            rect.height = (float) actualHeight / Screen.height;

            rect.x = (1f - rect.width) / 2f;
            rect.y = (1f - rect.height) / 2f;

            cam.rect = rect;
        }

        private void OnRenderImage(RenderTexture source, RenderTexture destination) {
            if (blit) {
                var buffer = RenderTexture.GetTemporary(renderWidth, renderHeight, -1);

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