using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UI;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class Level : MonoBehaviour {
	[SerializeField] private string levelName = "";

	public static Level Own => instance;
	private static Level instance = null;

	private LevelState state = LevelState.begin;
	public LevelState State => state;

	public delegate void ParameterAction<T>(T value);
	public ParameterAction<LevelState> onStateChanged;

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

	void ChangeState(LevelState newState) {
		state = newState;
		onStateChanged?.Invoke(state);
	}

	void Begin() {
		ChangeState(LevelState.begin);

		Play(); // TODO this is debug for starting the level right away
	}

	public void Play() {
		ChangeState(LevelState.play);
		PlayAll();
	}

	public void Pause() {
		ChangeState(LevelState.pause);
		PauseAll();
	}

	public void Win() {
		Debug.Log("you won :>");
		PauseAll();
		ChangeState(LevelState.win);
	}

	public void Lose() {
		Debug.Log("you lost :(");
		PauseAll();
		ChangeState(LevelState.lost);
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

	public void ToMenu() {
		SceneManager.LoadScene("Menu");
	}
}