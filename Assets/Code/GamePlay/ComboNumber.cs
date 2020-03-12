using UnityEngine;

namespace GamePlay {
    public class ComboNumber : MonoBehaviour {
        [SerializeField] private TextMesh text = null;

        public void Init(int number) {
            text.text = $"{number}";
        }
    }
}