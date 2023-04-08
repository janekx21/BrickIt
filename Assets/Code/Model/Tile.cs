using System;
using UnityEngine;

namespace Model {
    [Serializable]
    public struct Tile {
        public string type;
        public float rotation;
        public ColorType color;
        public Vector2Int position;
    }
}
