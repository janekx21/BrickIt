using System.Collections;
using System.Collections.Generic;
using UI.Views;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;
using Menu = UI.Menu.Menu;

[RequireComponent(typeof(Button))]
public class GotoView : MonoBehaviour {
    [SerializeField] private View @goto;

    private void Awake() {
        GetComponent<Button>().onClick.AddListener(() => FindObjectOfType<Menu>().ChangeState(@goto));
    }
}
