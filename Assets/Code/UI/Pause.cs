using System;
using UnityEngine;
using UnityEngine.UI;

namespace UI {
	public class Pause : MonoBehaviour {
		[SerializeField] private Button unpause = null;
		[SerializeField] private Button toMainMenu = null;
		[SerializeField] private Button toDesktop = null;

		private void Awake() {
			unpause.onClick.AddListener(() => { Level.Own.Play(); });
			toMainMenu.onClick.AddListener(() => {
				// TODO change scene
			});
			toDesktop.onClick.AddListener(() => {
				Application.Quit(0);
			});
		}
	}
}