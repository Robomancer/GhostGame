using UnityEngine;
using UnityEngine.UI;

public class InventoryView : View
{
    [SerializeField] private Button _backButton;
    [SerializeField] private Button _itemButton;

    public override void Initialize()
    {
        _backButton.onClick.AddListener(() => ViewManager.ShowLast());
        _itemButton.onClick.AddListener(() => ItemSpawn());

        //check user's saved inventory -> populate GetChild(4).GetChild(0-11) [order is bag, bPotion, gPotion, rPotion, bow, quiver, sceptre, shield, sword, decree, gEgg, rEgg]
    }

    public void ItemSpawn()
    {
        //check item > 0, call GameManagerScript.UserHideItem(_itemButton.item)
        ViewManager.Show<GameView>();
    }
}
