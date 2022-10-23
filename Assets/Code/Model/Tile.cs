using System;
using GamePlay;

namespace Model {
    [Serializable]
    public struct Tile {
        public string type;
        public float rotation;
        public ColorType color;
    }
}