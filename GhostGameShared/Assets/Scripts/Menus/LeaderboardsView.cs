using UnityEngine;
using UnityEngine.UI;

public class LeaderboardsView : View
{
    [SerializeField] private Button _backButton;

    public override void Initialize()
    {
        _backButton.onClick.AddListener(() => ViewManager.ShowLast());
    }
}
