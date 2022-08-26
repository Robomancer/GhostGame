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
        //Make a List of GPS Coords on Campus and Spawn Items at an Empty Coord 
        int gpsrand = Random.Range(0, gameManagerScript.GPSCords.Count);
        //Move to another GPS point
        transform.localPosition = new Vector3(gameManagerScript.GPSCords[gpsrand].x + Random.Range(0,4), transform.position.y, gameManagerScript.GPSCords[gpsrand].y + Random.Range(0, 4));
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
        gameManagerScript.SpawnItem(false);
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
