using UnityEngine;

namespace UI.Intro {
    public class Quitter : MonoBehaviour
    {
        private void Update()
        {
            if (Input.GetButton("Cancel")) {
                Application.Quit();
            }
        }
    }
}
