using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace UI {
	public class Win : MonoBehaviour {
		[SerializeField] private Button next = null;

		private void Awake() {
			next.onClick.AddListener(() => {
				// do shit
				Level.Level.Own.ToMenu();
			});
		}
	}
}