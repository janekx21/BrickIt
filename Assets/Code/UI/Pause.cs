using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace UI {
	public class Pause : MonoBehaviour {
		[SerializeField] private Button unpause = null;
		[SerializeField] private Button toMainMenu = null;
		[SerializeField] private Button toDesktop = null;

		[SerializeField] private SceneReference menuScene = null;

		private void Awake() {
			unpause.onClick.AddListener(() => { Level.Own.Play(); });
			toMainMenu.onClick.AddListener(() => {
				SceneManager.LoadScene(menuScene);
			});
			toDesktop.onClick.AddListener(() => {
				Application.Quit(0);
			});
		}
	}
}