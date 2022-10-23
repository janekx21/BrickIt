using System;
using LevelContext;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Util;

namespace UI {
    public class Pause : MonoBehaviour {
        [SerializeField] private Button unpause;
        [SerializeField] private Button toMainMenu;
        [SerializeField] private Button retry;

        private void Awake() {
            unpause.onClick.AddListener(() => { LevelContext.Level.Own.Play(); });
            toMainMenu.onClick.AddListener(() => { LevelContext.Level.Own.ToMenu(); });
            retry.onClick.AddListener(() => { LevelContext.Level.Own.Retry(); });
        }
    }
}