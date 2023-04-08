using Model;
using UnityEngine;

namespace GamePlay {
    /**
     * Actor that moves around the Level on its own
     */
    public interface IActor : IColored {
        void Init(Vector2 playerDirection, ColorType type);
        float GetDamage();
        void ChangeDirection(IDirectionChanger directionChanger);
        void FlipDirection();
        void Dash(Vector2 dir); // small acceleration
        void Die();
        void TeleportTo(Blocks.Block from, Blocks.Block to, Vector2 direction);
        Vector2 GetDirection();
        Vector2 GetPosition();
        void Combo();
        void ComboEnds();
        void ChangeSpeed(ISpeedChanger speedChanger);
        /**
         * Returns the current speed normalized to default speed = 1
         */
        float GetNormalizedSpeed();
    }
}