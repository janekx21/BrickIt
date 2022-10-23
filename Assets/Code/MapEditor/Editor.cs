using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Tilemaps;
using UnityEngine.UI;

namespace MapEditor {
    public class Editor : MonoBehaviour {
        public Tilemap tilemap;
        public Transform cursor;
        public Button rotateLeft;
        public Button rotateRight;

        public List<TileBase> placeableTiles = new();
        public RectTransform panel;
        public Button blockButtonPrefab;

        public GameObject spawnEffect;

        private TileBase currentTile;
        private Quaternion currentRotation = Quaternion.identity;

        void Start() {
            foreach (var placeableTile in placeableTiles) {
                var button = Instantiate(blockButtonPrefab, panel);
                button.onClick.AddListener(() => {
                    currentTile = placeableTile;
                    cursor.GetComponentInChildren<SpriteRenderer>().sprite = GetSprite(placeableTile);
                });

                button.GetComponent<Image>().sprite = GetSprite(placeableTile);
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

        void Update() {
            cursor.position = (Vector2) targetPosition + Vector2.one*.5f;
            cursor.rotation = currentRotation;

            foreach (Transform blockButton in panel) {
                blockButton.rotation = currentRotation;
            }

            var pointerEvent = new PointerEventData(EventSystem.current) { position = Input.mousePosition };
            var result = new List<RaycastResult>();
            EventSystem.current.RaycastAll(pointerEvent, result);
            if (result.Any()) return;
            
            if (Input.GetMouseButton(0)) {
                SetBlock(targetPosition, currentRotation, currentTile);
            }
            else if (Input.GetMouseButton(1)) {
                SetBlock(targetPosition, Quaternion.identity, null);
            }
        }

        private static Vector2Int targetPosition =>
            Vector2Int.FloorToInt(Camera.main.ScreenToWorldPoint(Input.mousePosition) * Vector2.one);

        private void SetBlock(Vector2Int position, Quaternion rotation, TileBase block) {
            if (tilemap.GetTile((Vector3Int) position) == block) return;
            tilemap.SetTile((Vector3Int) position, block);
            // todo normal tiles tilemap.SetTransformMatrix((Vector3Int) position, Matrix4x4.Rotate(rotation));
            tilemap.GetInstantiatedObject((Vector3Int) position).transform.rotation = rotation;
            
            // todo serialize and save
            // tiles = tilemap.GetTilesBlock(tilemap.cellBounds).Select(x => x?.name ?? "empty").ToList();
            if (spawnEffect) Instantiate(spawnEffect, position + Vector2.one * .5f, Quaternion.identity);
        }
    }
}