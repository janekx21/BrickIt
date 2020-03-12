﻿using UnityEngine;

namespace GamePlay {
    public interface IActor : IColored {
        void Init(Vector2 direction, Color color);
        float GetDamage();
        void ChangeDirection();
        void FlipDirection();
        void Dash(Vector2 direction); // small acceleration
        void Die();
        void TeleportTo(Blocks.Block from, Blocks.Block to, Vector2 direction);
        Vector2 GetDirection();
        Vector2 GetPosition();
        void Combo();
    }
}