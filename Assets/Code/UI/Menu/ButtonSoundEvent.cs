using System;
using UnityEngine;
using UnityEngine.EventSystems;
using Util;

namespace UI.Menu {
    [RequireComponent(typeof(SoundParticle))]
    public class ButtonSoundEvent : MonoBehaviour, ISelectHandler, IPointerEnterHandler, ISubmitHandler, IPointerClickHandler {
        [SerializeField] private AudioClip hover = null;
        [SerializeField] private AudioClip click = null;
        private SoundParticle source = null;

        private void Awake() {
            source = GetComponent<SoundParticle>();
        }

        public void OnSelect(BaseEventData eventData) {
            source.OneShot(hover);
        }
        
        public void OnPointerEnter(PointerEventData ped) {
            source.OneShot(hover);
        }

        public void OnSubmit(BaseEventData eventData) {
            source.OneShot(click);
        }

        public void OnPointerClick(PointerEventData eventData) {
            source.OneShot(click);
        }

    }
}