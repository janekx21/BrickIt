using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.ComTypes;
using Blocks;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.SceneManagement;
using UnityEngine.Tilemaps;

namespace Level {
    public class Overview {
        public struct OverviewObject {
            public Sprite sprite;
            public Vector2Int position;

            public OverviewObject(Sprite sprite, Vector2Int position) {
                this.sprite = sprite;
                this.position = position;
            }
        }

        public static void Generate(LevelObject level) {
            var scene = EditorSceneManager.OpenScene(level.scene.ScenePath, OpenSceneMode.Additive);
            var overview = new Texture2D(272, 160);

            if (scene.IsValid()) {
                var grid = scene.GetRootGameObjects().ToList().Find(o => o.GetComponent<Grid>());
                Assert.IsNotNull(grid, "grid != null");
                foreach (var t in Search(grid.transform)) {
                    Assert.IsNotNull(t.sprite);
                    // Debug.Log($"found {t.sprite.name} at {t.position}");
                    var tex = TextureFromSprite(t.sprite);
                    Vector2Int pos = t.position * 16;
                    overview.SetPixels(pos.x, pos.y, 16, 16, tex.GetPixels());
                }
            }
            else {
                Debug.LogWarning($"Scene not valid?\n{level.scene.ScenePath}");
            }

            byte[] bytes;
            bytes = overview.EncodeToPNG();

            var dir = Path.GetDirectoryName(level.scene.ScenePath);
            Assert.IsNotNull(dir);
            var path = Path.Combine(dir, "overview.png");
            File.WriteAllBytes(path, bytes);

            var texture = AssetDatabase.LoadAssetAtPath<Texture2D>(path);
            level.overview = texture;

            EditorSceneManager.CloseScene(scene, true);
        }

        static List<OverviewObject> Search(Transform transform) {
            var list = new List<OverviewObject>();

            int orthographicSize = Mathf.FloorToInt(Camera.main.orthographicSize);
            Vector2Int offset = new Vector2Int(Mathf.RoundToInt(orthographicSize * (16f / 9f)), orthographicSize);

            // pnew Vector2Int(9, 5);
            foreach (Transform t in transform) {
                var block = t.GetComponent<Block>();
                if (block) {
                    var ovo = block.ToOverviewObject();
                    ovo.position += offset;
                    list.Add(ovo);
                }

                var tilemap = t.GetComponent<Tilemap>();
                if (tilemap) {
                    foreach (var pos in tilemap.cellBounds.allPositionsWithin) {
                        var sprite = tilemap.GetSprite(pos);
                        // Vector2 worldPos = pos - transform.position - tilemap.tileAnchor;
                        // var cam = Camera.main;
                        // var orthographicSize = cam.orthographicSize;
                        // var size = new Vector2(-orthographicSize * cam.aspect, orthographicSize);
                        // var topLeftCorner = (Vector2) cam.transform.position + size;
                        // Vector2Int.FloorToInt(worldPos - topLeftCorner)
                        Vector2Int p = (Vector2Int) pos + offset;
                        if (sprite) {
                            list.Add(new OverviewObject(sprite, p));
                        }
                    }
                }

                list.AddRange(Search(t));
            }

            return list;
        }

        public static Texture2D TextureFromSprite(Sprite sprite) {
            if (Math.Abs(sprite.rect.width - sprite.texture.width) > .00001f) {
                Texture2D newText = new Texture2D((int) sprite.rect.width, (int) sprite.rect.height);
                Color[] newColors = sprite.texture.GetPixels(
                    (int) sprite.textureRect.x,
                    (int) sprite.textureRect.y,
                    (int) sprite.textureRect.width,
                    (int) sprite.textureRect.height);
                newText.SetPixels(newColors);
                newText.Apply();
                return newText;
            }

            return sprite.texture;
        }
    }
}