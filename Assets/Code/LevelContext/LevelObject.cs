using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Blocks;
using Model;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.SceneManagement;
using UnityEngine.Tilemaps;
using UnityEngine.UI;
using Util;

namespace LevelContext {
    [CreateAssetMenu]
    public class LevelObject : ScriptableObject {
        public string id;
        public SceneReference scene;
        public string levelName = "no name";
        public string levelAuthor = "no one";
        [Range(1, 20)] public int difficulty = 1;

        public Texture2D overview;
        public LevelData1 levelData = new();

        private void OnValidate() {
#if UNITY_EDITOR
            if (string.IsNullOrEmpty(id) || !GUID.TryParse(id, out _)) {
                id = GUID.Generate().ToString();
            }
#endif
        }

        protected bool Equals(LevelObject other) {
            return base.Equals(other) && id == other.id;
        }

        public override bool Equals(object obj) {
            if (ReferenceEquals(null, obj))
                return false;
            if (ReferenceEquals(this, obj))
                return true;
            if (obj.GetType() != this.GetType())
                return false;
            return Equals((LevelObject)obj);
        }

        public override int GetHashCode() {
            return HashCode.Combine(base.GetHashCode(), id);
        }

#if UNITY_EDITOR
        [ContextMenu("Rename Level")]
        public void RenameLevel() {
            var newName = levelName.Replace('?', '-');

            var levelObjectPath = AssetDatabase.GetAssetPath(this);
            var overviewPath = AssetDatabase.GetAssetPath(overview);
            var oldName = levelObjectPath.Split("/").Last().Split(".")[0];
            AssetDatabase.RenameAsset(levelObjectPath, newName);
            AssetDatabase.RenameAsset(scene.ScenePath, newName);
            AssetDatabase.RenameAsset(overviewPath, newName);

            // AssetDatabase.MoveAsset($"./{newName}.scene", $"../{newName}/{newName}.scene")
            var oldPath = Path.GetDirectoryName(levelObjectPath);
            if (oldPath == null) throw new Exception("Path was null");
            var newPath = oldPath + "/../" + newName;
            Directory.Move(oldPath, newPath);
            //TODO rename meta file
            // AssetDatabase.RenameAsset(oldPath + "/../" + oldName + ".meta", newName + ".meta");
        }

        [ContextMenu("Generate Overview")]
        private void GenerateOverview() {
            Overview.Generate(this);
        }

        [ContextMenu("Convert to Data Level")]
        public void ConvertToDataLevel() {
            var openScene = EditorSceneManager.OpenScene(scene.ScenePath, OpenSceneMode.Additive);
            Assert.IsTrue(openScene.IsValid(), "Scene is not valid");
            var grid = openScene.GetRootGameObjects().ToList().Find(o => o.GetComponent<Grid>());
            Assert.IsNotNull(grid, "grid != null, You got a level without a grid");
            
            var tileList = new List<Tile1>();
            foreach (Transform child in grid.transform) {
                var tilemap = child.GetComponent<Tilemap>();

                if (tilemap) {
                    foreach (var pos in tilemap.cellBounds.allPositionsWithin) {
                        var sprite = tilemap.GetSprite(pos);
                        if (sprite) {
                            tileList.Add(new Tile1(TileType.wall, 0, ColorType.@default, (Vector2Int)pos));
                        }
                    }

                    foreach (Transform t in tilemap.transform) {
                        var block = t.GetComponent<Block>();
                        Assert.IsNotNull(block, "block != null, There is a non block inside your tilemap");
                        var position = Vector2Int.FloorToInt(t.localPosition);
                        var rotation = t.rotation.eulerAngles.z;
                        var color = block.GetColorType();
                        var tile = block switch {
                            FlyThrough flyThrough => new Tile1(TileType.flyThrough, 0, color, position),
                            MultiHit multiHit => new Tile1(TileType.multiHit, 0, color, position,
                                multiHit.GetMaxHp()),
                            Normal normal => new Tile1(TileType.normal, 0, color, position),
                            ColorChanger colorChanger => new Tile1(TileType.colorChanger, 0, color, position),
                            Death death => new Tile1(TileType.death, 0, color, position),
                            DirectionChanger directionChanger => new Tile1(
                                directionChanger.GetDirection() == Direction.left
                                    ? TileType.directionChangerLeft
                                    : TileType.directionChangerRight, 0, color, position),
                            Spawner spawner => new Tile1(TileType.spawner, rotation, color, position),
                            SpeedChanger speedChanger => new Tile1(TileType.speedChanger, rotation, color, position),
                            Teleporter teleporter => new Tile1(TileType.teleporter, rotation, color, position),
                            _ => throw new ArgumentOutOfRangeException(nameof(block))
                        };
                        tileList.Add(tile);
                    }
                }
                else {
                    throw new Exception("Non tilemap inside grid");
                }
            }

            var levelScript = openScene.GetRootGameObjects().ToList().Find(o => o.GetComponent<Level>())
                .GetComponent<Level>();
            
            var camera = openScene.GetRootGameObjects().ToList().Find(o => o.GetComponent<Camera>());
            var text = camera.GetComponentsInChildren<Text>().First(x => x.text == "999");
            
            levelData = new LevelData1 {
                id = id,
                name = levelName,
                author = levelAuthor,
                data = tileList,
                size = new Vector2Int(levelScript.LevelWidth, levelScript.LevelHeight),
                version = "1",
                timerPosition = text.transform.position - grid.transform.position,
            };

            EditorUtility.SetDirty(this);

            if (SceneManager.sceneCount > 1) {
                EditorSceneManager.CloseScene(openScene, true);
            }

            AssetDatabase.Refresh();
        }
#endif
    }
}
