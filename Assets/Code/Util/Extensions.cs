using UnityEngine;

namespace Util {
    public static class Extensions {
        public static void PlayOverlapping(this AudioSource source) {
           source.PlayOneShot(source.clip);
        }
    }
}