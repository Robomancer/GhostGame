using UnityEngine;
using UnityEngine.UI;

public class RelicsView : View
{
    [SerializeField] private Button _backButton;

    public override void Initialize()
    {
        _backButton.onClick.AddListener(() => ViewManager.ShowLast());
    }
}
