using Level;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Util;

namespace UI.Intro {
    public class Credits : MonoBehaviour {

        [SerializeField] private Button backButton = null;
        [SerializeField] private SceneReference startScene = null;

        private void Awake() {
            backButton.onClick.AddListener(Back);
        }

        private void Update() {
            if (Input.GetButtonDown("Cancel")) {
                Back();
            }
        }

        void Back() {
            SceneManager.LoadScene(startScene); 
        }
    }
}