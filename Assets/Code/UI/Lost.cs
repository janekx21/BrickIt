using LevelContext;
using UnityEngine;
using UnityEngine.UI;

namespace UI {
    public class Lost : MonoBehaviour {
        [SerializeField] private Button retry;

        private void Awake() {
            retry.onClick.AddListener(() => { Level.Retry(); });
        }
    }
}