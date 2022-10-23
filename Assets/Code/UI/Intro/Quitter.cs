using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

public class Quitter : MonoBehaviour
{
    private void Update()
    {
        if (Input.GetButton("Cancel")) {
            Application.Quit();
        }
    }
}
