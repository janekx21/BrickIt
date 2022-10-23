using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace UI {
    public class Lost : MonoBehaviour {
        [SerializeField] private Button retry;

        private void Awake() {
            retry.onClick.AddListener(() => { LevelContext.Level.Own.Retry(); });
        }
    }
}