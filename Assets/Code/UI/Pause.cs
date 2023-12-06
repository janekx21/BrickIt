using LevelContext;
using UnityEngine;
using UnityEngine.UI;

namespace UI {
    public class Pause : MonoBehaviour {
        [SerializeField] private Button unpause;
        [SerializeField] private Button toMainMenu;
        [SerializeField] private Button retry;

        private void Awake() {
            unpause.onClick.AddListener(() => { LevelContext.Level.own.Play(); });
            toMainMenu.onClick.AddListener(() => { LevelContext.Level.own.ToMenu(); });
            retry.onClick.AddListener(() => { Level.own.Retry(); });
        }
    }
}
