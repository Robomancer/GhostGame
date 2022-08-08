using UnityEngine;
using UnityEngine.UI;

public class InventoryView : View
{
    [SerializeField] private Button _backButton;

    public override void Initialize()
    {
        _backButton.onClick.AddListener(() => ViewManager.ShowLast());
    }
}
