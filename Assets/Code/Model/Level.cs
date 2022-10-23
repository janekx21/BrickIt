using System;
using System.Collections.Generic;

namespace Model {
    [Serializable]
    public struct Level {
        public string name;
        public List<Tile> data;
    }
}