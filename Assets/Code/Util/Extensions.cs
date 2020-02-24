using UnityEngine;

namespace Util {
    public static class Extensions {
        public static void PlayOverlapping(this AudioSource source) {
            source.PlayOneShot(source.clip);
        }

        public static void PlayRandomPitch(this AudioSource source, float randomness) {
            source.pitch = 1 + Random.Range(-randomness, randomness);
            source.PlayOneShot(source.clip);
        }
    }
}