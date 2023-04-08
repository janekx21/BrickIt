using System;
using System.Collections.Generic;
using System.Linq;
using GamePlay;
using Model;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Tilemaps;
using UnityEngine.UI;
using Tile = UnityEngine.Tilemaps.Tile;

namespace MapEditor {
    public class Editor : MonoBehaviour {
        public Tilemap tilemap;
        public Transform cursor;
        public Button rotateLeft;
        public Button rotateRight;
        public RectTransform panel;
        public RectTransform colorPanel;

        public List<TileBase> placeableTiles = new();
        public Button blockButtonPrefab;
        public Button colorButtonPrefab;

        public GameObject spawnEffect;

        private TileBase currentTile;
        private Quaternion currentRotation = Quaternion.identity;
        private ColorType currentColor = ColorType.@default;

        private void Start() {
            foreach (var placeableTile in placeableTiles) {
                var button = Instantiate(blockButtonPrefab, panel);
                button.onClick.AddListener(() => {
                    currentTile = placeableTile;
                    cursor.GetComponent<SpriteRenderer>().sprite = GetSprite(placeableTile);
                });

                button.GetComponent<Image>().sprite = GetSprite(placeableTile);
            }

            foreach (var color in ColorConversion.allColors) {
                var button = Instantiate(colorButtonPrefab, colorPanel);
                button.onClick.AddListener(() => currentColor = color);
                button.GetComponent<Image>().color = ColorConversion.Convert(color);
            }

            rotateLeft.onClick.AddListener(() => currentRotation *= Quaternion.AngleAxis(-90f, Vector3.back));
            rotateRight.onClick.AddListener(() => currentRotation *= Quaternion.AngleAxis(+90f, Vector3.back));

            currentTile = placeableTiles.First();
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
            cursor.rotation = currentRotation;
            cursor.GetComponent<SpriteRenderer>().color = ColorConversion.Convert(currentColor);

            foreach (Transform blockButton in panel) {
                blockButton.rotation = currentRotation;
                blockButton.GetComponent<Image>().color = ColorConversion.Convert(currentColor);
            }

            var pointerEvent = new PointerEventData(EventSystem.current) {position = Input.mousePosition};
            var result = new List<RaycastResult>();
            EventSystem.current.RaycastAll(pointerEvent, result);
            if (result.Any()) return;

            if (Input.GetMouseButton(0)) {
                SetBlock(targetPosition, currentRotation, currentTile, currentColor);
            }
            else if (Input.GetMouseButton(1)) {
                SetBlock(targetPosition, Quaternion.identity, null, ColorType.@default);
            }
        }

        private static Vector2Int targetPosition =>
            Vector2Int.FloorToInt(Camera.main!.ScreenToWorldPoint(Input.mousePosition) * Vector2.one);

        private void SetBlock(Vector2Int position, Quaternion rotation, TileBase block, ColorType color) {
            if (tilemap.GetTile((Vector3Int) position) == block) return;
            tilemap.SetTile((Vector3Int) position, block);
            var go = tilemap.GetInstantiatedObject((Vector3Int) position);
            if (go) {
                go.transform.rotation = rotation;
                go.GetComponent<IColored>()?.SetColorType(color);
            }
            else {
                tilemap.SetTransformMatrix((Vector3Int) position, Matrix4x4.Rotate(rotation));
            }


            Save();
            // todo serialize and save
            // tiles = 
            if (spawnEffect) Instantiate(spawnEffect, position + Vector2.one * .5f, Quaternion.identity);
        }

        private void Save() {
            tilemap.CompressBounds();
            var tiles = GetTiles(tilemap).ToList();
            var level = new Model.Level {name = "foobar", data = tiles};
            Debug.Log(JsonUtility.ToJson(level));
        }

        private static IEnumerable<Model.Tile> GetTiles(Tilemap tilemap) {
            foreach (var position in tilemap.cellBounds.allPositionsWithin) {
                if (tilemap.HasTile(position)) {
                    var go = tilemap.GetInstantiatedObject(position);
                    if (go) {
                        yield return new Model.Tile {
                            type = go.name, color = go.GetComponent<IColored>().GetColorType(),
                            rotation = go.transform.rotation.eulerAngles.z
                        };
                    }
                    else {
                        yield return new Model.Tile {
                            type = tilemap.GetTile(position).name,
                            rotation = tilemap.GetTransformMatrix(position).rotation.eulerAngles.z
                        };
                    }
                }
                else {
                    yield return new Model.Tile {type = "empty"};
                }
            }
        }
    }
}
