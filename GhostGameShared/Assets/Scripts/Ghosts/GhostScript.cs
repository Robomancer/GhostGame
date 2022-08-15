using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GhostScript : MonoBehaviour
{
    GameManagerScript gameManagerScript;
    //Ghost Variables
    public string FavoriteItem;
    public string Name;

    // Ghost Functions
    private void Hide() 
    {
        // Vector3 GPSCoords;
        //Move to another GPS point
        // transform.localPosition = GPSCoords;
    }
    //AcceptItem
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "ITEM")
        {
            Accept(collision.gameObject);
        }
    }
    private IEnumerator Accept(GameObject Item) 
    {
        if (Item.name == FavoriteItem)
        {
            Destroy(Item);
            yield return new WaitForSeconds(5);
            Give();
        }
        else 
        {
            //Display Item Rejection

            //Decrease Favorability

            //Hide
            Hide();
        }
    }
    //GiveItem
    private void Give() 
    {
        //Choose Non-Favorite Item
        //Hide
        Hide();
        //Spawn at Ghost GPS Point
    }
    void Start() 
    {
        gameManagerScript = GetComponent<GameManagerScript>();
    }
    void Update() { }
}