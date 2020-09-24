using System;
using LevelContext;
using UnityEngine;

namespace UI {
    public class ConditionalPanel : Panel {
        [SerializeField] private LevelState ownState = LevelState.begin;

        private void Start() {
            OnUpdateLevelState(LevelContext.Level.Own.State);
            LevelContext.Level.Own.onStateChanged.AddListener(OnUpdateLevelState);
        }

        public override void OnUpdateLevelState(LevelState state) {
            gameObject.SetActive(state == ownState);
        }
    }
}