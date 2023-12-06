using System;
using System.Collections.Generic;
using System.Linq;
using Blocks;
using GamePlay;
using Model;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using UnityEngine.Tilemaps;
using UnityEngine.U2D;
using UnityEngine.UI;
using Util;

namespace LevelContext {
    public class Level : MonoBehaviour {
        [Serializable]
        public struct PlaceableTile {
            public TileType type;
            public TileBase tile;
        }

        public static Level own { get; private set; }

        [SerializeField] private Camera levelCamera;
        [SerializeField] private Tilemap levelTilemap;
        [SerializeField] private Text timer;
        [SerializeField] private List<PlaceableTile> placeableTiles;
        [SerializeField] private int levelWidth = 17;
        [SerializeField] private int levelHeight = 10;

        [Tooltip("leave this empty if you dont use this inside the editor")] [SerializeField]
        private LevelObject forceLevel;

        public int LevelWidth => levelWidth;
        public int LevelHeight => levelHeight;

        public LevelState state { get; private set; } = LevelState.begin;

        private bool cancelIsDown = true;

        public class OnLevelStateChanged : UnityEvent<LevelState> { }

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
        private int combo;
        private float comboTimer;
        [SerializeField] private float comboTime = 1f;


        public bool ready { get; set; }

        private void Awake() {
            Assert.IsNull(own);
            own = this;

            var pixelPerfectCamera = levelCamera.GetComponent<PixelPerfectCamera>();
            pixelPerfectCamera.refResolutionX = levelWidth * 16;
            pixelPerfectCamera.refResolutionY = levelHeight * 16;
        }

        private void Start() {
            if (forceLevel) {
                LoadLevelData(forceLevel.levelData);
            }

            Begin();
        }

        private void LoadLevelData(LevelData1 levelData1) {
            levelTilemap.ClearAllTiles();

            foreach (var t in levelData1.data) {
                var tileBase = placeableTiles.Find(x => x.type == t.type).tile;
                SetBlock(t.position, Quaternion.Euler(0, 0, t.rotation), tileBase, t.color);
            }
            // TODO center camera
            //levelTilemap.boun
            levelTilemap.CompressBounds();

            levelTilemap.origin =  -(Vector3Int)levelData1.size;
            levelTilemap.size = (Vector3Int)levelData1.size * 2;
            levelTilemap.FloodFill(-(Vector3Int)levelData1.size, placeableTiles[0].tile);

            timer.transform.position = levelData1.timerPosition + Vector2.one * .5f;
            levelTilemap.CompressBounds();
        }

        private void SetBlock(Vector2Int position, Quaternion rotation, TileBase block, ColorType color) {
            if (levelTilemap.GetTile((Vector3Int)position) == block) return;
            levelTilemap.SetTile((Vector3Int)position, block);
            var go = levelTilemap.GetInstantiatedObject((Vector3Int)position);
            if (go) {
                go.transform.rotation = rotation;
                go.GetComponent<IColored>()?.SetColorType(color);
            }
            else {
                levelTilemap.SetTransformMatrix((Vector3Int)position, Matrix4x4.Rotate(rotation));
            }
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

        public void FixedUpdate() {
            if (comboTimer <= 0 && combo > 0) {
                ComboEnds();
            }

            comboTimer = Mathf.Clamp01(comboTimer - Time.fixedDeltaTime);
        }

        public void Init(LevelObject levelObject) {
            Debug.Log("init level object");
            Debug.Log(levelObject);
            ownLevelObject = levelObject;
            LoadLevelData(levelObject.levelData);
        }

        public void ChangeState(LevelState newState) {
            state = newState;
            onStateChanged?.Invoke(state);
        }

        private void Begin() {
            timeSinceStart = 0;
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
            ComboEnds();
            ChangeState(LevelState.win);
            PauseAll();

            using var data = SaveData.Load();
            Debug.Log(data.done);
            Debug.Log(ownLevelObject);
            data.done.Add(ownLevelObject.id);
        }

        public void Lose() {
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
            var menu = FindMenuScene();
            if (menu == null) {
                SceneManager.LoadScene("Menu");
            }
            else {
                for (var i = 0; i < SceneManager.sceneCount; i++) {
                    var scene = SceneManager.GetSceneAt(i);
                    if (scene == menu) continue;
                    SceneManager.UnloadSceneAsync(scene);
                }

                SceneManager.SetActiveScene(menu.Value);
                foreach (var rootGameObject in menu.Value.GetRootGameObjects()) {
                    rootGameObject.SetActive(true);
                }
            }
        }

        private static Scene? FindMenuScene() {
            for (var i = 0; i < SceneManager.sceneCount; i++) {
                var scene = SceneManager.GetSceneAt(i);
                if (scene.name == "Menu") return scene;
            }

            return null;
        }

        public void Retry() {
            //var buildIndex = SceneManager.GetActiveScene().buildIndex;
            //SceneManager.LoadScene(buildIndex);
            var player = FindObjectOfType<Player>();
            if (player) {
                Destroy(player.gameObject);
            }
            LoadLevelData(ownLevelObject.levelData);
            Begin();
        }

        /**
         * Gets called when some positive action takes place that keeps the combo alive
         */
        public void Combo() {
            combo++;
            comboTimer = comboTime;
        }

        public void ComboEnds() {
            ApplyCombo();
            combo = 0;
        }

        public void ApplyCombo() {
            if (combo > maxCombo) {
                maxCombo = combo;
            }

            comboScore += Mathf.FloorToInt(Mathf.Pow((combo - 1) * 100, 2));
        }
    }
}
