using System;
using LevelContext;
using UnityEngine;

namespace UI {
    public class ConditionalPanel : Panel {
        [SerializeField] private LevelState ownState = LevelState.Begin;

        private void Start() {
            OnUpdateLevelState(Level.Own.State);
            Level.Own.onStateChanged.AddListener(OnUpdateLevelState);
        }

        public override void OnUpdateLevelState(LevelState state) {
            gameObject.SetActive(state == ownState);
        }
    }
}