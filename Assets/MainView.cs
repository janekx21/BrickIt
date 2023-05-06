using UI.Menu;
using UnityEngine;
using UnityEngine.UI;

public class MainView : MonoBehaviour {
    [SerializeField] private Button campaignButton;
    [SerializeField] private Button onlineButton;
    [SerializeField] private Menu menu;
    [SerializeField] private GameObject campaignView;
    [SerializeField] private GameObject onlineView;

    private void Awake()
    {
        campaignButton.onClick.AddListener(() => menu.ChangeState(campaignView));
        onlineButton.onClick.AddListener(() => menu.ChangeState(onlineView));
    }
}
