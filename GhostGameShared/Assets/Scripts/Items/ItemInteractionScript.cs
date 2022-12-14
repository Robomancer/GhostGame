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
    GameManagerScript gameManagerScript;

    //ITEM Variables
    public string Name;
    bool IsHidden = true;
    //Owner goes here

    IEnumerator DwellTimer(GameObject obj) 
    {
        DwelledObject = obj;
        yield return new WaitForSeconds(DwellTime);
        if (obj == DwelledObject)
        {
            gameManagerScript.UserFoundItem(DwelledObject);
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
    private void Awake() 
    {
        gameManagerScript = FindObjectOfType<GameManagerScript>();
    }
    void Start() { }
    void Update() { }
}