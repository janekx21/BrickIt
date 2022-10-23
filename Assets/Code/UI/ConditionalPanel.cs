using System.Collections;
using System.Linq;
using LevelContext;
using UnityEngine;

namespace UI {
    [RequireComponent(typeof(CanvasGroup))]
    public class ConditionalPanel : Panel {
        [SerializeField] private LevelState ownState = LevelState.begin;
        [SerializeField] private bool useAnimation;
        [SerializeField] private AnimationCurve alphaCurve = new();
        [SerializeField] private AnimationCurve scaleCurve = new();

        private CanvasGroup group;

        private void Start() {
            OnUpdateLevelState(Level.own.State);
            Level.own.onStateChanged.AddListener(OnUpdateLevelState);
            group = GetComponent<CanvasGroup>();
        }

        public override void OnUpdateLevelState(LevelState state) {
            gameObject.SetActive(state == ownState);
            if (useAnimation) {
                StartCoroutine(FadeRoutine());
            }
        }

        private IEnumerator FadeRoutine() {
            for (float t = 0; t < alphaCurve.keys.Last().time; t+=Time.deltaTime) {
                group.alpha = alphaCurve.Evaluate(t);
                transform.localScale = scaleCurve.Evaluate(t) * Vector3.one;
                yield return null;
            }
        }
    }
}