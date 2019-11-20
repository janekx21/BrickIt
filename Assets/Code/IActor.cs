using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IActor {
	float GetDamage();
	Color GetColor();
	void ChangeDirection();
	void FlipDirection();
}
