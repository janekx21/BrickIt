﻿using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace UI {
	public class Lost : MonoBehaviour {
		[SerializeField] private Button retry = null;

		private void Awake() {
			retry.onClick.AddListener(() => { SceneManager.LoadScene(SceneManager.GetActiveScene().name); });
		}
	}
}