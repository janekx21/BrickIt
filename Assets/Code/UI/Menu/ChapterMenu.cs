﻿using System;
using Level;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

namespace UI.Menu {
	public class ChapterMenu : MonoBehaviour {
		[SerializeField] private ChapterObject chapterObject = null;
		[SerializeField] private LevelList list = null;

        public class OnLevelAction : UnityEvent<LevelObject>{}
        
		private void Awake() {
            var changeEvent = new OnLevelAction();
            changeEvent.AddListener(LoadLevel);
			list.Init(chapterObject.levels, changeEvent);
		}

        private void Update() {
            if (Input.GetKeyDown(KeyCode.Escape)) { 
			    SceneManager.LoadScene("Menu");
            }
        }

        private void LoadLevel(LevelObject level) {
			SceneManager.LoadScene(level.scene.ScenePath);
		}
	}
}