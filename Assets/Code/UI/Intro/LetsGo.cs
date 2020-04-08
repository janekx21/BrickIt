using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Util;

public class LetsGo : MonoBehaviour {
    [SerializeField] private Button letsGo = null;
    [SerializeField] private SceneReference mainScene = null;

    private void Awake() {
        letsGo.onClick.AddListener(() => { SceneManager.LoadScene(mainScene); });
    }
}
