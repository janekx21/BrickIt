using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Blocks;
using GamePlay;
using Model;
using UnityEditor;
using UnityEditor.Tilemaps;
using UnityEngine;
using UnityEngine.Tilemaps;
using Tile = UnityEngine.Tilemaps.Tile;


[System.Serializable]
public struct TileToBlock {
    public Tile tile;
    public Block block;
}

[CreateAssetMenu]
[CustomGridBrush(false, true, false, "Brushes/BlockBrush")]
public class BlockBrush : GridBrush {
    public List<TileToBlock> conversion = new List<TileToBlock>();

    private Vector3Int prev_position = Vector3Int.zero;
    private GameObject prev_brushTarget = null;

    private Block current = null;

    public ColorType CurrentColorType {
        get => colorType;
        set => colorType = value;
    }

    private ColorType colorType = ColorType.@default;

    public override void Paint(GridLayout grid, GameObject brushTarget, Vector3Int position) {
        // if (position == prev_position) {
        //     return;
        // }

        prev_position = position;
        if (brushTarget) {
            prev_brushTarget = brushTarget;
        }

        brushTarget = prev_brushTarget;

        // Do not allow editing palettes
        if (brushTarget.layer == 31)
            return;

        if (current) {
            GameObject instance = (GameObject) PrefabUtility.InstantiatePrefab(current.gameObject);
            if (instance != null) {
                Erase(grid, brushTarget, position);

                Undo.MoveGameObjectToScene(instance, brushTarget.scene, "Paint Prefabs");
                Undo.RegisterCreatedObjectUndo(instance, "Paint Prefabs");
                instance.transform.SetParent(brushTarget.transform);
                instance.transform.position =
                    grid.LocalToWorld(grid.CellToLocalInterpolated(position + (Vector3) Vector2.one * .5f));
                instance.GetComponent<Block>()?.SetColorType(colorType);

                Selection.activeTransform = instance.transform;
            }
        }
        else {
            Erase(grid, brushTarget, position);
        }
    }

    public override void BoxFill(GridLayout gridLayout, GameObject brushTarget, BoundsInt position) {
        foreach (var pos in position.allPositionsWithin) {
            Paint(gridLayout, brushTarget, pos);
        }
    }

    public override void Erase(GridLayout grid, GameObject brushTarget, Vector3Int position) {
        if (brushTarget) {
            prev_brushTarget = brushTarget;
        }

        brushTarget = prev_brushTarget;
        // Do not allow editing palettes
        if (brushTarget.layer == 31)
            return;

        Transform erased = GetObjectInCell(grid, brushTarget.transform, position);
        if (erased != null)
            Undo.DestroyObjectImmediate(erased.gameObject);
    }

    private static Transform GetObjectInCell(GridLayout grid, Transform parent, Vector3Int position) {
        int childCount = parent.childCount;
        Vector3 min = grid.LocalToWorld(grid.CellToLocalInterpolated(position));
        Vector3 max = grid.LocalToWorld(grid.CellToLocalInterpolated(position + Vector3Int.one));
        Bounds bounds = new Bounds((max + min) * .5f, max - min);

        for (int i = 0; i < childCount; i++) {
            Transform child = parent.GetChild(i);
            if (bounds.Contains(child.position))
                return child;
        }

        return null;
    }

    public override void Pick(GridLayout gridLayout, GameObject brushTarget, BoundsInt position, Vector3Int pickStart) {
        base.Pick(gridLayout, brushTarget, position, pickStart);
        Tile tile = brushTarget.GetComponent<Tilemap>().GetTile(position.position) as Tile;
        Block block = conversion.Find(ttb => ttb.tile == tile).block;
        current = block;
    }

    public override void Select(GridLayout gridLayout, GameObject brushTarget, BoundsInt position) {
        var l = new List<Transform>();
        foreach (Vector3Int pos in position.allPositionsWithin) {
            var t = GetObjectInCell(gridLayout, brushTarget.transform, pos);
            if (t) {
                l.Add(t);
            }
        }

        Selection.objects = l.ConvertAll(input => input.gameObject).ToArray();
    }

    [CustomEditor(typeof(BlockBrush))]
    public class BlockBrushEditor : GridBrushEditor {
        private BlockBrush CurrentBlockBrush => target as BlockBrush;

        /// <summary>
        /// Callback for painting the inspector GUI for the PrefabBrush in the Tile Palette.
        /// The PrefabBrush Editor overrides this to have a custom inspector for this Brush.
        /// </summary>
        public override void OnPaintInspectorGUI() {
            GUILayout.BeginHorizontal();
            foreach (ColorType colorType in Enum.GetValues(typeof(ColorType)) as ColorType[]) {
                GUI.backgroundColor = ColorConversion.Convert(colorType);
                if (GUILayout.Button(ObjectNames.NicifyVariableName(colorType.ToString()))) {
                    CurrentBlockBrush.CurrentColorType = colorType;
                }
            }
            // CurrentBlockBrush.CurrentColorType = EditorGUILayout.ColorField(CurrentBlockBrush.CurrentColorType);
            // if (GUILayout.Button("Reset Color")) {
            //     CurrentBlockBrush.CurrentColorType = ColorType.defaultColorType;
            // }

            GUILayout.EndHorizontal();

            GUI.backgroundColor = Color.white;
            
            GUILayout.BeginHorizontal();
            if (GUILayout.Button("Rotate Left \u21ba")) {
                foreach (var t in Selection.transforms) {
                    if (t.TryGetComponent(out MultiHit multiHit) && multiHit.GetMaxHp() > 2) {
                        multiHit.SetMaxHp(multiHit.GetMaxHp() - 1);
                    }
                    else {
                        rot(90, t);
                    }
                }
            }

            if (GUILayout.Button("Rotate Right \u21bb")) {
                foreach (var t in Selection.transforms) {
                    if (t.TryGetComponent(out MultiHit multiHit) && multiHit.GetMaxHp() < 9) {
                        multiHit.SetMaxHp(multiHit.GetMaxHp() + 1);
                    }
                    else {
                        rot(-90, t);
                    }
                }
            }

            if (GUILayout.Button("Reset Rotation")) {
                foreach (var t in Selection.transforms) {
                    t.rotation = Quaternion.identity;
                }
            }

            GUILayout.EndHorizontal();
        }

        void rot(float deg, Transform t) {
                t.rotation *= Quaternion.AngleAxis(deg, Vector3.forward);
        }
    }
}