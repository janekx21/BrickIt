using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Util;

namespace UI.Intro {
    public class Credits : MonoBehaviour {

        [SerializeField] private Button backButton;
        [SerializeField] private SceneReference startScene;

        private bool cancelIsDown;
        
        private void Awake() {
            backButton.onClick.AddListener(Back);
        }

        private void Update() {
            if (Input.GetAxisRaw("Cancel") != 0) {
                if (cancelIsDown) return;
                Back();
                cancelIsDown = true;
            }
            else {
                cancelIsDown = false;
            }
        }

        private void Back() {
            SceneManager.LoadScene(startScene); 
        }
    }
}
