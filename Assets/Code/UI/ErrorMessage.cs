using UnityEngine;
using UnityEngine.UI;

namespace UI {
    public class ErrorMessage : MonoBehaviour
    {
        [SerializeField] private Button closeButton;
        void Start()
        {
            closeButton.onClick.AddListener(() => gameObject.SetActive(false)); 
        }
    }
}
