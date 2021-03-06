#if UNITY_EDITOR
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Blocks;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.Tilemaps;

namespace LevelContext {
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
            // offset to paint blocks at the edge - x offset is a bit weird
            var edgeOffset = new Vector2Int(0, 8);
            var scene = EditorSceneManager.OpenScene(level.scene.ScenePath, OpenSceneMode.Additive);
            var levelScript = scene.GetRootGameObjects().ToList().Find(o => o.GetComponent<Level>()).GetComponent<Level>();
            var levelSize = new Vector2Int(16 * levelScript.LevelWidth, 16 * levelScript.LevelHeight);

            var overview =
                new Texture2D(levelSize.x + 2 * edgeOffset.y, levelSize.y + 2 * edgeOffset.y) {
                    filterMode = FilterMode.Point
                };
            var length = overview.width * overview.height;
            var blackArray = new Color[length];
            overview.SetPixels(blackArray);
            overview.Apply();


            if (scene.IsValid()) {
                var grid = scene.GetRootGameObjects().ToList().Find(o => o.GetComponent<Grid>());
                Assert.IsNotNull(grid, "grid != null");
                foreach (var t in Search(grid.transform)) {
                    Assert.IsNotNull(t.sprite);
                    // Debug.Log($"found {t.sprite.name} at {t.position}");
                    var tex = TextureFromSprite(t.sprite);
                    Vector2Int pos = t.position * 16 + Vector2Int.RoundToInt(grid.transform.position * 2) * 8 + edgeOffset;
                    // well then dont draw if you cant
                    if (pos.x + 16 <= overview.width && pos.x >= 0 && pos.y + 16 <= overview.height && pos.y >= 0) {
                        overview.SetPixels(pos.x, pos.y, 16, 16, tex.GetPixels());
                    }
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
            Assert.IsNotNull(texture);
            level.overview = texture;

            EditorUtility.SetDirty(level);

            if (EditorSceneManager.sceneCount > 1) {
                EditorSceneManager.CloseScene(scene, true);
            }

            AssetDatabase.Refresh();
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
#endif