﻿using System.IO;
using System.Linq;
using Blocks;
using GamePlay;
using UnityEditor;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using UnityEngine.U2D;
using Util;

namespace LevelContext {
    public class Level : MonoBehaviour {
        public static Level Own => instance;
        private static Level instance;

        [SerializeField] private Camera levelCamera;
        [SerializeField] private int levelWidth = 17;
        [SerializeField] private int levelHeight = 10;
        
        public int LevelWidth => levelWidth;
        public int LevelHeight => levelHeight;
     
        private LevelState state = LevelState.Begin;
        public LevelState State => state;
        private bool cancelIsDown = true;
        private bool ready;

        public class OnLevelStateChanged : UnityEvent<LevelState> {
        }

        public readonly OnLevelStateChanged onStateChanged = new();

        private float timeSinceStart;
        private int comboScore;
        private int maxCombo;
        private LevelObject ownLevelObject;

        private float timeScoreBase = 1000000f;
        private float factor = 1.02f;

        public float TimeSinceStart => timeSinceStart;
        // private int TimeScore => Mathf.FloorToInt(Mathf.Max(1 - Mathf.Log10(timeSinceStart * 10 / 999), 0) * 200);
        public int TimeScore => Mathf.FloorToInt(timeScoreBase * Mathf.Pow(factor, -timeSinceStart));
        public int Score => TimeScore + comboScore;
        public int MaxCombo => maxCombo;

        public bool Ready {
            get => ready;
            set => ready = value;
        }

        private void Awake() {
            Assert.IsNull(instance);
            instance = this;

            var pixelPerfectCamera = levelCamera.GetComponent<PixelPerfectCamera>();
            pixelPerfectCamera.refResolutionX = levelWidth * 16;
            pixelPerfectCamera.refResolutionY = levelHeight * 16;
        }

        private void Start() {
            Begin();
        }
        
        private void OnDrawGizmos() {
            Gizmos.DrawWireCube(transform.position, new Vector3(levelWidth, levelHeight, 0));
        }

        private void Update() {
#if DEBUG
            if (Input.GetKey(KeyCode.O) && Input.GetKeyDown(KeyCode.Delete)) {
                PlayerPrefs.DeleteAll();
            }
#endif
            
            switch (state) {
                case LevelState.Play:
                    if (Input.GetAxisRaw("Cancel") != 0) {
                        if (!cancelIsDown) {
                            Pause();

                            cancelIsDown = true;
                        }
                    }
                    else {
                        cancelIsDown = false;
                    }

                    break;

                case LevelState.Pause:
                    if (Input.GetAxisRaw("Cancel") != 0) {
                        if (!cancelIsDown) {
                            Play();

                            cancelIsDown = true;
                        }
                    }
                    else {
                        cancelIsDown = false;
                    }

                    break;

                case LevelState.Begin:
                    if (ready && (Input.anyKey || Input.touchCount > 0)) {
                        Play();
                        FindObjectOfType<Spawner>().Spawn();
                    }

                    break;
            }

            if (state == LevelState.Play) {
                timeSinceStart += Time.deltaTime;
            }
        }

        public void Init(LevelObject levelObject) {
            ownLevelObject = levelObject;
        }

        public void ChangeState(LevelState newState) {
            state = newState;
            onStateChanged?.Invoke(state);
        }

        private void Begin() {
            ChangeState(LevelState.Begin);

            // Play Animation and halt until start button is pressed
            // Play(); // TODO this is debug for starting the level right away
        }

        public void Play() {
            ChangeState(LevelState.Play);
            PlayAll();
        }

        public void Pause() {
            ChangeState(LevelState.Pause);
            PauseAll();
        }

        public void Win() {
            Debug.Log("you won :>");
            ChangeState(LevelState.Win);
            PauseAll();

            using var data = SaveData.Load();
            data.done.Add(ownLevelObject.id);
        }

        public void Lose() {
            Debug.Log("you lost :(");
            ChangeState(LevelState.Lost);
            PauseAll();
        }

        private void PlayAll() {
            var objs = FindObjectsOfType<Entity>().OfType<IPausable>();
            foreach (var obj in objs) {
                if (obj.isPaused()) {
                    obj.play();
                }
            }
        }

        private void PauseAll() {
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

        public void Retry() {
            var buildIndex = SceneManager.GetActiveScene().buildIndex;
            SceneManager.LoadScene(buildIndex);
            Debug.Log(buildIndex);
        }

        public void ApplyCombo(int combo) {
            if (combo > maxCombo) {
                maxCombo = combo;
            }
            comboScore += Mathf.FloorToInt(Mathf.Pow((combo - 1) * 100, 2));
        }
    }
}