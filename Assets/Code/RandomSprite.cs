using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class RandomSprite : MonoBehaviour {
    [SerializeField] Sprite[] possibleSprites = new Sprite[0];

    public void Start() {
        var ren = GetComponent<SpriteRenderer>();
        
        var position = transform.position;
        int index = Mathf.FloorToInt(position.x * 131 + position.y * 17);
        ren.sprite = possibleSprites[index % possibleSprites.Length];
    }

    private void OnDrawGizmos() {
        Gizmos.DrawIcon(transform.position, "CubeInCircle.png");
    }
}