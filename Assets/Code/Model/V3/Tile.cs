using UnityEngine;

namespace Model.V3 {
    [System.Serializable]
    public struct Tile {
        public TileType type;
        public float rotation; // only relevant for 
        public ColorType color;
        public Vector2Int position;
        public int hitCount; // only relevant for multi hit blocks

        public Tile(TileType type, float rotation, ColorType color, Vector2Int position, int hitCount = -1) {
            this.type = type;
            this.rotation = rotation;
            this.color = color;
            this.position = position;
            this.hitCount = hitCount;
        }
    }
}
