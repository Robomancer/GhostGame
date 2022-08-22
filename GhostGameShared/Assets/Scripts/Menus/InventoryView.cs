using UnityEngine;
using UnityEngine.UI;

public class InventoryView : View
{
    [SerializeField] private Button _backButton;
    [SerializeField] private Button _bagItemButton;
    [SerializeField] private Button _bpotItemButton;
    [SerializeField] private Button _gpotItemButton;
    [SerializeField] private Button _rpotItemButton;
    [SerializeField] private Button _bowItemButton;
    [SerializeField] private Button _quiverItemButton;
    [SerializeField] private Button _sceptreItemButton;
    [SerializeField] private Button _shieldItemButton;
    [SerializeField] private Button _swordItemButton;
    [SerializeField] private Button _decreeItemButton;
    [SerializeField] private Button _geggItemButton;
    [SerializeField] private Button _reggItemButton;

    [SerializeField] private Image _amounts;
    [SerializeField] private GameManagerScript GM;

    int[] invAmounts;

    private void Awake()
    {
        //GM = FindObjectOfType<GameManagerScript>();

        for (int i = 0; i < 12; i++)
        {
            invAmounts[i] = int.Parse(_amounts.transform.GetChild(i).gameObject.GetComponent<Text>().text);
        }
    }

    public override void Initialize()
    {
        _backButton.onClick.AddListener(() => ViewManager.ShowLast());

        _bagItemButton.onClick.AddListener(() => ItemSpawn(0));
        _bpotItemButton.onClick.AddListener(() => ItemSpawn(1));
        _gpotItemButton.onClick.AddListener(() => ItemSpawn(2));
        _rpotItemButton.onClick.AddListener(() => ItemSpawn(3));
        _bowItemButton.onClick.AddListener(() => ItemSpawn(4));
        _quiverItemButton.onClick.AddListener(() => ItemSpawn(5));
        _sceptreItemButton.onClick.AddListener(() => ItemSpawn(6));
        _shieldItemButton.onClick.AddListener(() => ItemSpawn(7));
        _swordItemButton.onClick.AddListener(() => ItemSpawn(8));
        _decreeItemButton.onClick.AddListener(() => ItemSpawn(9));
        _geggItemButton.onClick.AddListener(() => ItemSpawn(10));
        _reggItemButton.onClick.AddListener(() => ItemSpawn(11));

        //check user's saved inventory -> populate GetChild(4).GetChild(0-11) [order is bag, bPotion, gPotion, rPotion, bow, quiver, sceptre, shield, sword, decree, gEgg, rEgg]
    }

    public void ItemSpawn(int invItem)
    {
        //check item > 0
        if(invAmounts[invItem] > 0)
        {
            //calls gm
            GM.UserHideItem(GM.items[invItem]);

            //sets user item -1
            invAmounts[invItem] -= 1;
            _amounts.transform.GetChild(invItem).gameObject.GetComponent<Text>().text = invAmounts[invItem].ToString();

            //changes to game view
            ViewManager.Show<GameView>();
        }
        else
        {
            //no item? implement sick burn
        }        
    }
}
