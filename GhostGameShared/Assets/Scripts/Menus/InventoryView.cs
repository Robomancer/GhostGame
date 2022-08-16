using UnityEngine;
using UnityEngine.UI;

public class InventoryView : View
{
    [SerializeField] private Button _backButton;
    [SerializeField] private Button _itemButton;

    public override void Initialize()
    {
        _backButton.onClick.AddListener(() => ViewManager.ShowLast());
        //_itemButton.onClick.AddListener(() => GameManagerScript.UserHideItem(_itemButton.item));
    }
}
