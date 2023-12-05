using System;
using UnityEngine;

namespace Model {
    /// <summary>
    /// This is a tile with version 1. The version will be used for migrations.
    /// </summary>
    [Serializable]
    public struct Tile1 {
        public TileType type;
        public float rotation; // only relevant for 
        public ColorType color;
        public Vector2Int position;
        public int hitCount; // only relevant for multi hit blocks

        public Tile1(TileType type, float rotation, ColorType color, Vector2Int position, int hitCount = -1) {
            this.type = type;
            this.rotation = rotation;
            this.color = color;
            this.position = position;
            this.hitCount = hitCount;
        }
    }
}
