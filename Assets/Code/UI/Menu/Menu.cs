using System.Collections;
using System.Linq;
using UI.Views;
using UnityEngine;
using UnityEngine.SceneManagement;
using Util;

namespace UI.Menu {
    public class Menu : MonoBehaviour {
        [SerializeField] private AnimationCurve cameraCurve;
        [SerializeField] private View defaultView;

        private void Start() {
            using var data = SaveData.GetHandle();
            var initialView = Id.FindById(data.save.lastMenuView);
            ChangeState(initialView == null ? defaultView : initialView.GetComponent<View>());
            // This is already done i think: StartCoroutine(ChangeStateRoutine(views.Main));
        }

        private void Update() {
            if (Input.GetButtonDown("Cancel")) {
                Application.Quit();
            }
        }

        public void ChangeState(View next) {
            using var saveData = SaveData.GetHandle();
            saveData.save.lastMenuView = next.GetComponent<Id>().id;
            StartCoroutine(ChangeStateRoutine(next));
        }

        public IEnumerator ChangeStateRoutine(View nextState) {
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
    }
}
