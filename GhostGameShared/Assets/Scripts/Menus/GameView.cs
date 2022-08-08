using UnityEngine;
using UnityEngine.UI;

public class GameView : View
{
    [SerializeField] private Button _inventoryButton;
    [SerializeField] private Button _favorButton;
    [SerializeField] private Button _relicsButton;
    [SerializeField] private Button _leaderboardsButton;
    [SerializeField] private Button _optionsButton;

    public override void Initialize()
    {
        _inventoryButton.onClick.AddListener(() => ViewManager.Show<InventoryView>());
        _favorButton.onClick.AddListener(() => ViewManager.Show<FavorView>());
        _relicsButton.onClick.AddListener(() => ViewManager.Show<RelicsView>());
        _leaderboardsButton.onClick.AddListener(() => ViewManager.Show<LeaderboardsView>());
        _optionsButton.onClick.AddListener(() => ViewManager.Show<OptionsView>());
    }
}
