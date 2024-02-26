using System;
using UnityEngine;

namespace Model {
    [Serializable]
    public class KeyValuePair<K, V> {
        public K key;
        public V value;
    }
}
