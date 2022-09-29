using System;
using System.Collections;
using System.Linq;
using LevelContext;
using UnityEngine;
using UnityEngine.Serialization;

namespace UI {
    [RequireComponent(typeof(CanvasGroup))]
    public class ConditionalPanel : Panel {
        [SerializeField] private LevelState ownState = LevelState.Begin;
        [SerializeField] private bool useAnimation;
        [SerializeField] private AnimationCurve alphaCurve = new();
        [SerializeField] private AnimationCurve scaleCurve = new();

        private CanvasGroup group;

        private void Start() {
            OnUpdateLevelState(Level.Own.State);
            Level.Own.onStateChanged.AddListener(OnUpdateLevelState);
            group = GetComponent<CanvasGroup>();
        }

        public override void OnUpdateLevelState(LevelState state) {
            gameObject.SetActive(state == ownState);
            if (useAnimation) {
                StartCoroutine(FadeRoutine());
            }
        }

        IEnumerator FadeRoutine() {
            for (float t = 0; t < alphaCurve.keys.Last().time; t+=Time.deltaTime) {
                group.alpha = alphaCurve.Evaluate(t);
                transform.localScale = scaleCurve.Evaluate(t) * Vector3.one;
                yield return null;
            }
        }
    }
}