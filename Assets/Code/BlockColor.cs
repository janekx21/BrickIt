using System;
using System.Collections.Generic;
using UnityEngine;

public abstract class BlockColor : Block {

    [SerializeField] private Color color = Color.white;

    private void OnDrawGizmos() {
        Gizmos.color = color;
        for (float i = .8f; i < 1f; i += .01f) {
            Gizmos.DrawWireCube(transform.position, new Vector3(1, 1, 0) * i);
        }
    }

    protected Color GetColor() {
        return color;
    }

    protected void SetColor(Color color) {
        this.color = color;
    }
}