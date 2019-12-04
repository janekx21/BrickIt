using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IActor {
	float GetDamage();
	Color GetColor();
    void SetColor(Color color);
	void ChangeDirection();
	void FlipDirection();
	void Dash(Vector2 direction); // small acceleration
	void Die();
}
