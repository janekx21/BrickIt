using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Assertions;

public class Level : MonoBehaviour {
	[SerializeField] private string levelName = "";

	public static Level Own => instance;
	private static Level instance = null;

	private LevelState state = LevelState.begin;

	private void Awake() {
		Assert.IsNull(instance);
		instance = this;
	}

	void Start() {
		Begin();
	}

	void Update() {
		switch (state) {
			case LevelState.play:
				if (Input.GetKeyDown(KeyCode.Escape)) {
					Pause();
				}

				break;
			case LevelState.pause:
				if (Input.GetKeyDown(KeyCode.Escape)) {
					Play();
				}

				break;
		}
	}

	void Begin() {
		state = LevelState.begin;

		Play(); // TODO this is debug for starting the level right away
	}

	void Play() {
		state = LevelState.play;
		PlayAll();
	}

	void Pause() {
		state = LevelState.pause;
		PauseAll();
	}

	public void Win() {
		Debug.Log("you won :>");
		PauseAll();
	}

	public void Lose() {
		Debug.Log("you lost :(");
		PauseAll();
	}

	void PlayAll() {
		var objs = FindObjectsOfType<Entity>().OfType<IPausable>();
		foreach (var obj in objs) {
			if (obj.isPaused()) {
				obj.play();
			}
		}
	}

	void PauseAll() {
		var objs = FindObjectsOfType<Entity>().OfType<IPausable>();
		foreach (var obj in objs) {
			if (!obj.isPaused()) {
				obj.pause();
			}
		}
	}
}