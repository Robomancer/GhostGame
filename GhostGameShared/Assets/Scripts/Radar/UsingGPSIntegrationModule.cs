using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UsingGPSIntegrationModule : MonoBehaviour
{
    GPSIntegrationModule myGPS;
    public GameObject Marker;
    // Start is called before the first frame update
    void Start()
    {
        myGPS = GameObject.FindObjectOfType<GPSIntegrationModule>();
        if(myGPS == null)
        {
            throw new System.Exception("ERROR:  Scene does not contain a GPSIntegrationModule Script.  Please add one!");
        }
        myGPS.OnGPSModuleStart += GPSStarted;
    }

    private void OnDestroy()
    {
        myGPS.OnGPSModuleStart -= GPSStarted;
    }

    public void GPSStarted()
    {
        myGPS.writeToScreenConsole("Receieved event, spawning objects!");
        myGPS.CreatObjectFromGPS(Marker, new Vector2(28.149755f, -81.852517f));
        myGPS.CreatObjectFromGPS(Marker, new Vector2(28.14792f, -81.852211f));
        myGPS.CreatObjectFromGPS(Marker, new Vector2(28.150132f, -81.852498f));
        GameObject temp = myGPS.CreatObjectFromGPS(Marker, new Vector2(28.149768f, -81.852033f));
        myGPS.writeToScreenConsole("Created closes marker at: " + temp.transform.position.ToString("F5"));
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
