using System;
using System.Collections;
using System.Collections.Generic;
using UI;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Menu : MonoBehaviour {
	[SerializeField] private List<LevelObject> levelObjects = new List<LevelObject>();
	[SerializeField] private LevelList list = null;

	private void Awake() {
		list.Init(levelObjects.ToArray(), LoadLevel);
	}

	[ContextMenu("Find All Levels")]
	private void FindAllLevelObjects() {
		levelObjects.Clear();
		var allLevelPaths = AssetDatabase.FindAssets("t:LevelObject");
		foreach (var guid in allLevelPaths) {
			var levelPath = AssetDatabase.GUIDToAssetPath(guid);
			var lo = AssetDatabase.LoadAssetAtPath<LevelObject>(levelPath);
			levelObjects.Add(lo);
		}
	}

	private void LoadLevel(LevelObject level) {
		SceneManager.LoadScene(level.scene.ScenePath);
	}
}