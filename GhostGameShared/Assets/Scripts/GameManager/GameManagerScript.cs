using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManagerScript : MonoBehaviour
{
    public Item[] items;
    ClientAPI clientAPI;
    void Start()
    {
        clientAPI = FindObjectOfType<ClientAPI>();
        StartCoroutine(HideItem());
    }
    //Item Spawning
    public IEnumerator HideItem() 
    {
        yield return StartCoroutine(clientAPI.Get("localhost:9080/hiddenitems/getAllHiddenItems"));
        Debug.Log(clientAPI.output);
        //yield return StartCoroutine(clientAPI.Post("localhost: 5000", item));
        //Item Generation
        int choose = Random.Range(0,items.Length);
        //Select Item Prefab
        Item item = items[choose];
        //Random GPS Point on Campus
        //Make a List of GPS Coords on Campus and Spawn Items at an Empty Coord
        // Vector3 GPSCoords;
        //Spawn at GPS
        Instantiate(item.Prefab,new Vector3(1,0,1),Quaternion.identity);
        //Add Record of Item to Server
    }
    //Item Hidden/Found
    public void SpawnItem() 
    {
        //Select Item from PLayer
        //Current GPS Point of Item
        //IsHidden = true;
        //Point Calculation?
    }
}
