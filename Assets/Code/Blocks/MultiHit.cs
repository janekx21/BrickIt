using System;
using GamePlay;
using UnityEngine;

namespace Blocks {
    public class MultiHit : Brick {
        [SerializeField] [Range(2, 9)] private int maxHp = 2;
        private int hp = -1;

        [SerializeField] private Sprite[] sprites = null;

        public override void Awake() {
            base.Awake();
            hp = maxHp;
            ren.sprite = sprites[hp - 1];
        }

        public override void OnValidate() {
            base.OnValidate();
            ren.sprite = sprites[maxHp - 1];
        }

        public int GetMaxHp() {
            return maxHp;
        }

        public void SetMaxHp(int maxHp) {
            this.maxHp = maxHp;
            OnValidate();
        }
        
        public override void Hit(IActor maker) {
            base.Hit(maker);
            if (ColorsMatch(maker)) {
                hp--;
                if (hp <= 0) {
                    Break(maker);
                }
                else {
                    ren.sprite = sprites[hp - 1];
                }
            }
        }

    }
}