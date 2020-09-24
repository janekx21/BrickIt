using System;
using LevelContext;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Util;

namespace UI {
    public class Pause : MonoBehaviour {
        [SerializeField] private Button unpause = null;
        [SerializeField] private Button toMainMenu = null;
        [SerializeField] private Button retry = null;

        private void Awake() {
            unpause.onClick.AddListener(() => { LevelContext.Level.Own.Play(); });
            toMainMenu.onClick.AddListener(() => { LevelContext.Level.Own.ToMenu(); });
            retry.onClick.AddListener(() => { LevelContext.Level.Own.Retry(); });
        }
    }
}