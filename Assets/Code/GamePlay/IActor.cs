using UnityEngine;

namespace GamePlay {
	public interface IActor {
		float GetDamage();
		Color GetColor();
		void SetColor(Color color);
		void ChangeDirection();
		void FlipDirection();
		void Dash(Vector2 direction); // small acceleration
		void Die();
		void TeleportTo(Blocks.Block from ,Blocks.Block to, Vector2 direction);
		Vector2 GetDirection();
        Vector2 GetPosition();
    }
}
