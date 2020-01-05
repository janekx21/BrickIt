using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IPausable {
	void play();
	void pause();

	bool isPaused();
}