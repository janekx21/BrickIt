using UnityEngine;

namespace UI {
    public abstract class Panel : MonoBehaviour {
        public abstract void OnUpdateLevelState(LevelState state);
    }
}
