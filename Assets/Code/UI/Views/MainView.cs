using UnityEngine;
using UnityEngine.UI;

namespace UI.Views {
    public class MainView : View {
        [SerializeField] private Button campaignButton;
        [SerializeField] private Button onlineButton;
        [SerializeField] private Menu.Menu menu;
        [SerializeField] private View campaignView;
        [SerializeField] private View onlineView;

        private void Awake()
        {
            campaignButton.onClick.AddListener(() => menu.ChangeState(campaignView));
            onlineButton.onClick.AddListener(() => menu.ChangeState(onlineView));
        }
    }
}
