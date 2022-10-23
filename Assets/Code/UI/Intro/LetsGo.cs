using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Util;

namespace UI.Intro {
    public class LetsGo : MonoBehaviour {
        [SerializeField] private Button letsGo;
        [SerializeField] private SceneReference mainScene;

        private void Awake() {
            letsGo.onClick.AddListener(() => { SceneManager.LoadScene(mainScene); });
        }
    }
}