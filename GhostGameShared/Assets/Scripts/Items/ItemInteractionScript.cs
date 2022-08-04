using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using Microsoft.MixedReality.Toolkit.Input;
public class ItemInteractionScript : BaseFocusHandler
{
    public float DwellTime = 10;
    GameObject DwelledObject;
    bool Dwell = false;

    //ITEM Variables
    public enum ItemType {Cross, Book, Flower, Coins};
    bool IsHidden = true;
    int Points = 0;
    //Owner goes here
    IEnumerator DwellTimer(GameObject obj) 
    {
        DwelledObject = obj;
        yield return new WaitForSeconds(DwellTime);
        if (obj == DwelledObject)
        {
            Dwell = true;
        }
    }
    public override void OnFocusEnter(FocusEventData eventData)
    {
        //OnFocusDwell
        StartCoroutine(DwellTimer(eventData.selectedObject));
    }
    public override void OnFocusExit(FocusEventData eventData)
    {
        StopAllCoroutines();
        DwelledObject = null;
    }
    // Start is called before the first frame update
    private void Awake()
    {
    }
    void Start()
    {

    }
    void Update() 
    {

    }
}