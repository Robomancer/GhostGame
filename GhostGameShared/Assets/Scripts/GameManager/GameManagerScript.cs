using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManagerScript : MonoBehaviour
{
    public Item[] items;
    ClientAPI clientAPI;
    bool NewItem = false;
    void Start()
    {
        clientAPI = FindObjectOfType<ClientAPI>();
       // StartCoroutine(HideItem());
    }
    void Heartbeat() 
    {
        if (NewItem) 
        {
            //Heartbeat stuff I guess
            AllItemsOnRadar();
            NewItem = !NewItem;
        }
    }

    public IEnumerator AllItemsOnRadar() 
    {
        // Radar Team
        yield return StartCoroutine(clientAPI.Get("localhost:9080/hiddenitems/getAllHiddenItems"));
        // add all items to radar use cords 
    }

    //Item Spawning all items 
    public IEnumerator SpawnItem(bool random) 
    {
        //Item Generation
        int choose = Random.Range(0, items.Length);
        //Select Item Prefab
        Item item = items[choose];
        if (random)
        {
            //Random GPS Point on Campus
            //Make a List of GPS Coords on Campus and Spawn Items at an Empty Coord
            // Vector3 GPSCoords;
            //Spawn at GPS
            Instantiate(item.Prefab, new Vector3(1, 0, 1), Quaternion.identity);
        }
        else 
        {
            //Random GPS Point on Campus
            //Spawn Object in front
            //Spawn at GPS
            Instantiate(item.Prefab,Vector3.forward + new Vector3(0,0,0.2f), Quaternion.identity);
        }
        //Add Record of Item to Server
        yield return StartCoroutine(clientAPI.Post("localhost:9080/hiddenitems/addHiddenItem",item));
        NewItem = !NewItem;
    }

    //Item Hidden/Found player inventory 
    public IEnumerator UserHideItem(Item item) 
    {
        //Current GPS Point of Item
        //IsHidden = true;
        yield return StartCoroutine(clientAPI.Post("localhost:9080/hiddenitems/addHiddenUserItem",item));
        NewItem = !NewItem;
    }

    public IEnumerator UserFoundItem(GameObject obj) 
    {
        //Select Item from PLayer
        Item item = new Item(obj.name);
        item.Prefab = obj.gameObject;
        //Current GPS Point of Item
        //IsHidden = false;
        //Point Calculation?
        yield return StartCoroutine(clientAPI.Post("localhost:9080/hiddenitems/checkLocation",item));
        NewItem = !NewItem;
    }


    public IEnumerator UserScore() 
    {
        yield return StartCoroutine(clientAPI.Get("localhost:9080/user/getScore"));
        // print numbers from output 
    }

    public IEnumerator leaderBoard() 
    {
        yield return StartCoroutine(clientAPI.Get("localhost:9080/user/leaderBoard"));
        // print numbers from output 
    }
}
