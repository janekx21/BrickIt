using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Util {
    public static class Extensions {
        public static void PlayOverlapping(this AudioSource source) {
            source.PlayOneShot(source.clip);
        }

        public static void PlayRandomPitch(this AudioSource source, float randomness, float offset = 1f) {
            source.pitch = offset + Random.Range(-randomness, randomness);
            source.Play();
        }

        public static void PlayRandomPitchOverlapping(this AudioSource source, float randomness) {
            source.pitch = 1 + Random.Range(-randomness, randomness);
            source.PlayOneShot(source.clip);
        }

        public static Quaternion Rotation(this Vector2 direction) {
            return Quaternion.AngleAxis(Vector2.SignedAngle(Vector2.up, direction), Vector3.forward);
        }

        public static IEnumerable<Vector3Int> AllPositions(this BoundsInt bounds) {
            foreach (var position in bounds.allPositionsWithin) {
                yield return position;
            }
        }

        public static Dictionary<K, V> AsDictionary<K,V>(this List<Model.KeyValuePair<K, V>> value) {
            return value.ToDictionary(x => x.key, x => x.value);
        }
        
        public static List<Model.KeyValuePair<K, V>> AsList<K,V>(this Dictionary<K, V> value) {
            return value.Select(x => new Model.KeyValuePair<K, V> { key = x.Key, value = x.Value }).ToList();
        }
    }
}
