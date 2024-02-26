using System.Collections.Generic;
using UnityEngine;

namespace Model.V3 {
    [System.Serializable]
    public class Save : VersionedData {
        public List<KeyValuePair<string, float>> scrollPositions;
        public List<string> done; // id's
        public string lastMenuView; // id
        public string lastChapterPlayed; // id
        public List<HighscoreEntry> highScores;
    }
}
