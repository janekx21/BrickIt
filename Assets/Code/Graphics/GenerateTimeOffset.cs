using System;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Graphics {
    public class GenerateTimeOffset : MonoBehaviour {
        private static readonly int Property = Shader.PropertyToID("_TimeOffset");

        [Range(0, 10)] [SerializeField] private float lowest;
        [Range(0, 10)] [SerializeField] private float highest = 1;

        [SerializeField]
        private SpriteRenderer[] renderers = Array.Empty<SpriteRenderer>();

        private void Start() {
            var offset = Random.Range(lowest, highest);
            foreach (var ren in renderers) {
                ren.material.SetFloat(Property, offset);
            }
        }
    }
}
