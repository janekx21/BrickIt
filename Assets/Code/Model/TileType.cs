using System;

namespace Model {
    // todo use this for serialisation
    [Serializable]
    public enum TileType {
        empty = 0,
        wall = 1,
        normal = 2,
        colorChanger = 4,
        multiHit = 5,
        flyThrough = 6,
        directionChangerRight = 7,
        directionChangerLeft = 8,
        speedChanger = 9,
        death = 10,
        teleporter = 11,
        spawner = 12
    }
}
