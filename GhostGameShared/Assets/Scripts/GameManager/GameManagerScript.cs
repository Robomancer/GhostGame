using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManagerScript : MonoBehaviour
{
    public Item[] items;
    ClientAPI clientAPI;
    GPSIntegrationModule gpsIntegrationModule;
    bool NewItem = false;
    public List<Vector2> GPSCords;
    void Start()
    {
        clientAPI = FindObjectOfType<ClientAPI>();
        gpsIntegrationModule = FindObjectOfType<GPSIntegrationModule>();
        // GPS List
        GPSCords.Add(new Vector2(28.15026932515052f, -81.84946512471909f));
        GPSCords.Add(new Vector2(28.14949643411042f, -81.8476391932996f));
        GPSCords.Add(new Vector2(28.14836530473714f, -81.84648222281147f));
        GPSCords.Add(new Vector2(28.150139662989265f, -81.8507175788629f));
        // StartCoroutine(HideItem());
    }
    private void Awake()
    {
        if (NewItem)
        {
            //Heartbeat stuff I guess 
            AllItemsOnRadar();
            NewItem = !NewItem;
        }
    }

    IEnumerator Heartbeat() 
    {
        StartCoroutine(SpawnItem(true));
        yield return new WaitForSeconds(.1f);
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
            int gpsrand = Random.Range(0, GPSCords.Count);
            //Make a List of GPS Coords on Campus and Spawn Items at an Empty Coord 
            // Vector3 GPSCoords; 
            //Spawn at GPS 
            Instantiate(item.Prefab, GPSCords[gpsrand], Quaternion.identity);
        }
        else 
        {
            //Random GPS Point on Campus 
            //Spawn Object in front 
            //Spawn at GPS 
            Instantiate(item.Prefab, gpsIntegrationModule.ToGPS(gpsIntegrationModule.CurrentGPSPosition), Quaternion.identity);
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
        Instantiate(item.Prefab, gpsIntegrationModule.ToGPS(gpsIntegrationModule.CurrentGPSPosition),Quaternion.identity);
        NewItem = !NewItem;
    }

    public IEnumerator UserFoundItem(GameObject obj) 
    {
        /*I Expect a Bug and will annoy Dr. Towle of Mon for insight here*/
        //Select Item from PLayer
        Item item = new Item(obj.name);
        item.Prefab = obj.gameObject;
        //Current GPS Point of Item
        //IsHidden = false;
        //Point Calculation?
        yield return StartCoroutine(clientAPI.Post("localhost:9080/hiddenitems/checkLocation",item));
        Destroy(obj);
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
