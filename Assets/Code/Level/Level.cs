using System.IO;
using System.Linq;
using Blocks;
using GamePlay;
using UnityEditor;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using Util;

namespace Level {
    public class Level : MonoBehaviour {
        public static Level Own => instance;
        private static Level instance = null;

        private LevelState state = LevelState.begin;
        public LevelState State => state;
        private bool cancelIsDown = false;

        public class OnLevelStateChanged : UnityEvent<LevelState> {
        }

        public OnLevelStateChanged onStateChanged = new OnLevelStateChanged();

        private float timeSinceStart = 0;
        private int comboScore = 0;
        private LevelObject ownLevelObject = null;

        public float TimeSinceStart => timeSinceStart;
        private int TimeScore => Mathf.FloorToInt(Mathf.Max(1 - Mathf.Log10(timeSinceStart * 10 / 999), 0) * 200);
        public int Score => TimeScore + comboScore;

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

                case LevelState.pause:
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

                case LevelState.begin:
                    if (Input.anyKey || Input.touchCount > 0) {
                        Play();
                        FindObjectOfType<Spawner>().Spawn();
                    }

                    break;
            }

            if (state == LevelState.play) {
                timeSinceStart += Time.deltaTime;
            }
        }

        public void Init(LevelObject levelObject) {
            ownLevelObject = levelObject;
        }

        void ChangeState(LevelState newState) {
            state = newState;
            onStateChanged?.Invoke(state);
        }

        void Begin() {
            ChangeState(LevelState.begin);

            // Play Animation and halt until start button is pressed
            // Play(); // TODO this is debug for starting the level right away
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

            using (var data = SaveData.Load()) {
                data.done.Add(ownLevelObject);
            }
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

        public void Retry() {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }

        public void ApplyCombo(int combo) {
            comboScore += Mathf.FloorToInt(Mathf.Pow(combo - 1, 2));
        }
    }
}