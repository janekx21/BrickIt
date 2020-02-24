using System;
using Level;
using UnityEngine;

namespace UI {
	public class ConditionalPanel : Panel {
		[SerializeField] private LevelState ownState = LevelState.begin;
		
		private void Start() {
			OnUpdateLevelState(Level.Level.Own.State);
			Level.Level.Own.onStateChanged.AddListener(OnUpdateLevelState);
		}

		public override void OnUpdateLevelState(LevelState state) {
			gameObject.SetActive(state == ownState);
		}
	}
}