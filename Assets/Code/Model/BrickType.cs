using System;

namespace Model {
    // todo use this for serialisation
    [Serializable]
    public enum BrickType {
        empty,
        wall,
        normal,
        colored,
        colorChanger,
        multiHit,
        flyThrough,
        rotateRight,
        rotateLeft,
        speed,
        death,
        portal,
        spawn
    }
}