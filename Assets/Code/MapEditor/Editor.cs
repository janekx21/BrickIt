using System;
using System.Collections.Generic;
using System.Linq;
using Blocks;
using GamePlay;
using LevelContext;
using Model;
using Model.V3;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Tilemaps;
using UnityEngine.UI;
using Util;
using Tile = UnityEngine.Tilemaps.Tile;

namespace MapEditor {
    public class Editor : MonoBehaviour {
        public Tilemap tilemap;
        public Transform cursor;
        public Button rotateLeft;
        public Button rotateRight;
        public Toggle deleteToggle;
        public RectTransform panel;
        public RectTransform colorPanel;

        public List<Model.KeyValuePair<TileType, TileBase>> placeableTiles = new();
        public Dictionary<TileType, TileBase> placeableTilesDict => placeableTiles.AsDictionary();
        public Dictionary<TileBase, TileType> placeableTilesDictSwapped => placeableTiles.AsDictionarySwapped();
        public Button blockButtonPrefab;
        public Button colorButtonPrefab;

        public GameObject spawnEffect;

        public string lastSave = "not saved yet";
        
        [Tooltip("leave this empty if you dont use this inside the editor")] [SerializeField]
        private LevelObject forceLevel;

        private class EditorTile {
            public Button Button;
            public TileBase Tile;
            public ColorType Color;
            public Quaternion Rotation;
        }

        private EditorTile currentBlock;
        private bool deleteMode;

        private void Start() {
            var buttons = placeableTilesDict.Values.Select(placeableTile => {
                var tile = new EditorTile {
                    Button = Instantiate(blockButtonPrefab, panel),
                    Tile = placeableTile,
                    Rotation = Quaternion.identity,
                    Color = ColorType.@default
                };
                
                tile.Button.onClick.AddListener(() => {
                    currentBlock = tile;
                    cursor.GetComponent<SpriteRenderer>().sprite = GetSprite(currentBlock.Tile);
                });

                tile.Button.GetComponent<Image>().sprite = GetSprite(placeableTile);
                return tile;
            }).ToList();

            foreach (var color in ColorConversion.allColors) {
                var button = Instantiate(colorButtonPrefab, colorPanel);
                button.onClick.AddListener(() => {
                    currentBlock.Button.image.color = ColorConversion.Convert(color);
                    currentBlock.Color = color;
                });
                button.GetComponent<Image>().color = ColorConversion.Convert(color);
            }

            rotateLeft.onClick.AddListener(() => {
                currentBlock.Button.transform.rotation *= Quaternion.AngleAxis(-90f, Vector3.back);
                currentBlock.Rotation *= Quaternion.AngleAxis(-90f, Vector3.back);
            });
            rotateRight.onClick.AddListener(() => {
                currentBlock.Button.transform.rotation *= Quaternion.AngleAxis(+90f, Vector3.back);
                currentBlock.Rotation *= Quaternion.AngleAxis(+90f, Vector3.back);
            });
            deleteToggle.onValueChanged.AddListener(value => deleteMode = value );

            buttons[2].Button.onClick.Invoke();
            
            SetTiles(forceLevel.levelData.data);
        }

        private static Sprite GetSprite(TileBase placeableTile) {
            var tileMDefaultSprite = placeableTile switch {
                Tile tile => tile.sprite,
                RuleTile tile => tile.m_DefaultSprite,
                _ => throw new NotImplementedException("Tile type not supported")
            };
            return tileMDefaultSprite;
        }

        private void Update() {
            cursor.position = targetPosition + Vector2.one * .5f;
            cursor.rotation = currentBlock.Rotation;
            cursor.GetComponent<SpriteRenderer>().color = ColorConversion.Convert(currentBlock.Color);
            
            var pointerEvent = new PointerEventData(EventSystem.current) { position = Input.mousePosition };
            var result = new List<RaycastResult>();
            EventSystem.current.RaycastAll(pointerEvent, result);
            if (result.Any()) return;

            if (Input.GetMouseButton(0)) {
                SetBlock(targetPosition, currentBlock.Rotation, deleteMode ? null : currentBlock.Tile, currentBlock.Color);
            }
            // impossible on mobile
            else if (Input.GetMouseButton(1)) {
                SetBlock(targetPosition, Quaternion.identity, null, ColorType.@default);
            }

            if (Input.GetKeyDown(KeyCode.L)) {
                Load(lastSave);
            }

            if (Input.GetKeyDown(KeyCode.K)) {
                lastSave = Save();
            }
            
            if (Input.GetKeyDown(KeyCode.Return)) {
                FindObjectOfType<Spawner>().Spawn();
            }
        }

        private static Vector2Int targetPosition =>
            Vector2Int.FloorToInt(Camera.main!.ScreenToWorldPoint(Input.mousePosition) * Vector2.one);

        private void SetBlock(Vector2Int position, Quaternion rotation, TileBase block, ColorType color) {
            if (tilemap.GetTile((Vector3Int)position) == block) return;
            tilemap.SetTile((Vector3Int)position, block);
            var go = tilemap.GetInstantiatedObject((Vector3Int)position);
            if (go) {
                go.transform.rotation = rotation;
                go.GetComponent<IColored>()?.SetColorType(color);
            }
            else {
                tilemap.SetTransformMatrix((Vector3Int)position, Matrix4x4.Rotate(rotation));
            }

            if (spawnEffect) Instantiate(spawnEffect, position + Vector2.one * .5f, Quaternion.identity);
        }

        private string Save() {
            tilemap.CompressBounds();
            var tiles = GetTiles(tilemap).ToList();
            var level = new LevelData { name = "foobar", data = tiles };
            return JsonUtility.ToJson(level);
        }

        private void Load(string data) {
            var level = JsonUtility.FromJson<LevelData>(data);
            SetTiles(level.data);
        }

        private IEnumerable<Model.V3.Tile> GetTiles(Tilemap tilemap) {
            foreach (var position in tilemap.cellBounds.AllPositions()) {
                if (tilemap.HasTile(position)) {
                    var go = tilemap.GetInstantiatedObject(position);
                    var tile = tilemap.GetTile(position);
                    if (go) {
                        yield return new Model.V3.Tile {
                            type = placeableTilesDictSwapped[tile],
                            rotation = go.transform.rotation.eulerAngles.z,
                            color = go.GetComponent<IColored>().GetColorType(),
                            position = (Vector2Int)position,
                            // TODO multi hit block
                        };
                    }
                    else {
                        yield return new Model.V3.Tile {
                            type = placeableTilesDictSwapped[tile],
                            rotation = tilemap.GetTransformMatrix(position).rotation.eulerAngles.z,
                            position = (Vector2Int)position
                        };
                    }
                }
                else {
                    yield return new Model.V3.Tile { type = TileType.empty, position = (Vector2Int)position };
                }
            }
        }


        private void SetTiles(IEnumerable<Model.V3.Tile> data) {
            foreach (var t in data) {
                var tileBase = t.type == TileType.empty ? null : placeableTilesDict[t.type];
                SetBlock(t.position, Quaternion.Euler(0, 0, t.rotation), tileBase, t.color);
            }
            tilemap.CompressBounds();
        }
    }
}
