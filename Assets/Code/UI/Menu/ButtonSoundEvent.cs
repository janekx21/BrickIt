using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using Util;

namespace UI.Menu {
    [RequireComponent(typeof(SoundParticle))]
    public class ButtonSoundEvent : MonoBehaviour, ISubmitHandler, IPointerClickHandler {
        [SerializeField] private AudioClip click;
        private SoundParticle source;

        private void Awake() {
            source = GetComponent<SoundParticle>();
        }

        public void OnSubmit(BaseEventData eventData) {
            if (TryGetComponent<Button>(out var button) && !button.interactable) return;
            source.OneShot(click);
        }

        public void OnPointerClick(PointerEventData eventData) {
            if (TryGetComponent<Button>(out var button) && !button.interactable) return;
            source.OneShot(click);
        }

    }
}
