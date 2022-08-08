using UnityEngine;
using UnityEngine.UI;

public class LoginView : View
{
    [SerializeField] private Button _loginButton;

    public override void Initialize()
    {
        _loginButton.onClick.AddListener(() => ViewManager.Show<GameView>());
    }
}
