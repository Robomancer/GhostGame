using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GhostScript : MonoBehaviour
{
    //Ghost Variables
    private enum GhostType { King, Queen, Knight, Archer, Bard, Mage };
    private string FavoriteItem;
    private string Name;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag.ToString() == FavoriteItem) 
        {
            // Increase Favorability
            // Give New Item
            // Bank Points
        }
    }
}
