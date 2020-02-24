using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Util;

namespace UI {
	public class Pause : MonoBehaviour {
		[SerializeField] private Button unpause = null;
		[SerializeField] private Button toMainMenu = null;
		[SerializeField] private Button toDesktop = null;

		private void Awake() {
			unpause.onClick.AddListener(() => { Level.Level.Own.Play(); });
			toMainMenu.onClick.AddListener(() => {
                Level.Level.Own.ToMenu();
			});
			toDesktop.onClick.AddListener(() => {
				Application.Quit(0);
			});
		}
	}
}