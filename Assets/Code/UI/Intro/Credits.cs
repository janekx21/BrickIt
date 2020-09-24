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

        private bool cancelIsDown = false;
        
        private void Awake() {
            backButton.onClick.AddListener(Back);
        }

        private void Update() {
            if (Input.GetAxisRaw("Cancel") != 0) {
                if (!cancelIsDown) {
                    Back();

                    cancelIsDown = true;
                }
            }
            else {
                cancelIsDown = false;
            }
        }

        void Back() {
            SceneManager.LoadScene(startScene); 
        }
    }
}