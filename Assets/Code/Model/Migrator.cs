namespace Model {
    /**
     * Migrates versions from 1 to 2, 2 to 3 and 3 to 4 and so on. The first version is created with init
     */
    public class Migrator {
        public static V3.Save Init() {
            // there will be a reset here so lets return an empty save result
            return new V3.Save {
                scrollPositions = null,
                done = null,
                lastMenuView = null,
                lastChapterPlayed = null,
                highScores = null
            };
        }
    }
}
