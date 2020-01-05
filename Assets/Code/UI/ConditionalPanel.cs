using System;
using UnityEngine;

namespace UI {
	public class ConditionalPanel : Panel {
		[SerializeField] private LevelState ownState = LevelState.begin;

		private void Start() {
			OnUpdateLevelState(Level.Own.State);
			Level.Own.onStateChanged += OnUpdateLevelState;
		}

		public override void OnUpdateLevelState(LevelState state) {
			gameObject.SetActive(state == ownState);
		}
	}
}