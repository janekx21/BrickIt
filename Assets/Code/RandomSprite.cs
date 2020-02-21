using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = System.Random;

[RequireComponent(typeof(SpriteRenderer))]
public class RandomSprite : MonoBehaviour {
    [SerializeField] Sprite[] possibleSprites = new Sprite[0];
    [SerializeField] private bool randomFlip = true;
    [SerializeField] private bool randomRotation = true;

    public void Start() {
        var ren = GetComponent<SpriteRenderer>();
        
        var position = transform.position;
        int index = Mathf.FloorToInt(position.x * 131 + position.y * 17);
        index = Math.Abs(index);
        ren.sprite = possibleSprites[index % possibleSprites.Length];
        if (randomFlip) {
            ren.flipX = Mathf.FloorToInt(position.y * 689 + position.x*7) % 2 == 0;
            ren.flipY = Mathf.FloorToInt(position.x * 876 + position.y*897) % 2 == 0;
        }

        if (randomRotation) {
            int angle = Mathf.FloorToInt(position.x * 981 + position.y * 119) % 4;
            transform.localRotation = Quaternion.AngleAxis(angle * 90, Vector3.forward);
        }
    }

    private void OnDrawGizmos() {
        Gizmos.DrawIcon(transform.position, "CubeInCircle.png");
    }
}