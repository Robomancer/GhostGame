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
    }
    //Item Spawning
    public void HideItem() 
    {
        StartCoroutine(clientAPI.Get("localhost: 5000"));
        // Item Generation
        int choose = Random.Range(0,items.Length);
        // Select Item Prefab
        Item item = items[choose];
        // Random GPS Point on Campus - Make a List of GPS Coords on Campus and Spawn Items at an Empty Coord
        Vector3 GPSCoords;
        // Spawn at GPS
        Instantiate(item.Prefab,GPSCoords,Quaternion.identity);
        // Add Record of Item to Server
        StartCoroutine(clientAPI.Post("localhost: 5000",item));
    }
    //Item Hidden/Found
    public void SpawnItem() 
    {
        // Select Item from PLayer
        // Current GPS Point of Item
        // IsHidden = true;
        // Point Calculation?
    }
}
