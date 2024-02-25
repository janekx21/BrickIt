using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace Model {
    /// <summary>
    /// This is a level with version 1. The version will be used for migrations.
    /// </summary>
    [Serializable]
    public struct LevelData1 {
        public string id;
        public string name;
        public string author;
        public Vector2Int size; // default is 17 x 10
        public string version;
        public Vector2 timerPosition;
        public List<Tile1> data;

        public LevelData1(string name, string author, IEnumerable<Tile1> data) {
            id = Guid.NewGuid().ToString();
            this.name = name;
            this.author = author;
            this.data = data.ToList();
            
            // TODO make size dynamic 
            // var width = data.Max(t => t.position.x) - data.Min(t => t.position.x);
            // var height = data.Max(t => t.position.y) - data.Min(t => t.position.y);
            size = new Vector2Int(17, 10);
            version = "1";
            timerPosition = Vector2Int.zero; // TODO load from level
        }
    }
}
