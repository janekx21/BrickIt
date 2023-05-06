using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using LevelContext;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Util;

namespace UI.Menu {
    public class Menu : MonoBehaviour {
        [Serializable]
        public struct Views {
            public GameObject Main;
            public GameObject Chaper;
            public GameObject Level;
            public GameObject Online;
        }

        [SerializeField] private Views views;
        [SerializeField] private AnimationCurve cameraCurve;

        private GameObject currentView;

        private void Start() {
            currentView = views.Main;
            // This is already done i think: StartCoroutine(ChangeStateRoutine(views.Main));
        }

        private void Update() {
            if (Input.GetButtonDown("Cancel")) {
                Back();
            }
        }

        public void ChangeState(GameObject next) {
            StartCoroutine(ChangeStateRoutine(next));
        }

        public IEnumerator ChangeStateRoutine(GameObject nextState) {
            var trans = Camera.main.transform;
            var start = trans.position;
            var target = nextState.transform.position;
            target.z = trans.position.z;

            for (var t = 0f; t < cameraCurve.keys.Last().time; t += Time.deltaTime) {
                trans.position = Vector3.Lerp(start, target, cameraCurve.Evaluate(t));
                yield return null;
            }

            trans.position = target;
        }

        private void Back() {
            if (currentView == views.Main) {
                SceneManager.LoadScene("Scenes/StartScreen");
            }
        }
    }
}
