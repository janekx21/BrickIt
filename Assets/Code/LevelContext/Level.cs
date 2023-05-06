using System.Linq;
using Blocks;
using GamePlay;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using UnityEngine.U2D;
using Util;

namespace LevelContext {
    public class Level : MonoBehaviour {
        public static Level? own { get; private set; }

        [SerializeField] private Camera levelCamera;
        [SerializeField] private int levelWidth = 17;
        [SerializeField] private int levelHeight = 10;
        
        public int LevelWidth => levelWidth;
        public int LevelHeight => levelHeight;

        public LevelState state { get; private set; } = LevelState.begin;

        private bool cancelIsDown = true;

        public class OnLevelStateChanged : UnityEvent<LevelState> {
        }

        public readonly OnLevelStateChanged onStateChanged = new();

        private int comboScore;
        private LevelObject ownLevelObject;

        private const float timeScoreBase = 1000000f;
        private const float factor = 1.02f;

        public float timeSinceStart { get; private set; }

        // private int TimeScore => Mathf.FloorToInt(Mathf.Max(1 - Mathf.Log10(timeSinceStart * 10 / 999), 0) * 200);
        public int timeScore => Mathf.FloorToInt(timeScoreBase * Mathf.Pow(factor, -timeSinceStart));
        public int score => timeScore + comboScore;
        public int maxCombo { get; private set; }

        public bool ready { get; set; }

        private void Awake() {
            Assert.IsNull(own);
            own = this;

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
                    if (ready && (Input.anyKey || Input.touchCount > 0)) {
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

        public void ChangeState(LevelState newState) {
            state = newState;
            onStateChanged?.Invoke(state);
        }

        private void Begin() {
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
            ChangeState(LevelState.win);
            PauseAll();

            using var data = SaveData.Load();
            data.done.Add(ownLevelObject);
        }

        public void Lose() {
            Debug.Log("you lost :(");
            ChangeState(LevelState.lost);
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
