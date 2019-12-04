using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEditor.Tilemaps;
using UnityEngine;
using UnityEngine.Tilemaps;


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

	public override void Paint(GridLayout grid, GameObject brushTarget, Vector3Int position) {
		if (position == prev_position) {
			return;
		}

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
				instance.transform.position = grid.LocalToWorld(grid.CellToLocalInterpolated(position + (Vector3)Vector2.one*.5f));
			}
		}
		else {
			Erase(grid, brushTarget, position);
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

	[CustomEditor(typeof(BlockBrush))]
	public class BlockBrushEditor : GridBrushEditor {
		private BlockBrush BlockBrush {
			get { return target as BlockBrush; }
		}

		private SerializedProperty blockPrefabs;
		private SerializedObject m_SerializedObject;

		protected override void OnEnable() {
			base.OnEnable();
			m_SerializedObject = new SerializedObject(target);
			// blockPrefabs = m_SerializedObject.FindProperty("blockPrefabs");
		}

		/// <summary>
		/// Callback for painting the inspector GUI for the PrefabBrush in the Tile Palette.
		/// The PrefabBrush Editor overrides this to have a custom inspector for this Brush.
		/// </summary>
		public override void OnPaintInspectorGUI() {
			m_SerializedObject.UpdateIfRequiredOrScript();
			// EditorGUILayout.PropertyField(blockPrefabs, true);
			m_SerializedObject.ApplyModifiedPropertiesWithoutUndo();
		}
	}
}