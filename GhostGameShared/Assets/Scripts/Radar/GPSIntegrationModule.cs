using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class GPSIntegrationModule : MonoBehaviour
{

    public GameObject UnityXRPlaySpace;
    Vector2 StartingGPS; //Will be lat, long (even though this is y,x)
    float StartingDirection;
    Vector2 LastGPS;
    Queue<Vector2> GPS_Records; //Will be lat, long (even though this is y,x)
    Queue<float> Altitude;
    float TotalDistance;
    float CurrentHeading;

    public float QueueSize = 500;

    private bool GPSStarted = false;
    private bool GPSModuleInitialized = false;
    public event Action OnGPSModuleStart;

    public int SampleSize = 3;
    public Vector3 CurrentGPSPosition;

    public float Accuracy = .1f;
    public float DelatDistance = .1f;

    public Text DebugConsole;
    void Start()
    {
        GPS_Records = new Queue<Vector2>();
        Altitude = new Queue<float>();
        StartCoroutine(SlowUpdate());
    }

    public void writeToScreenConsole(string s)
    {
        if (DebugConsole != null)
        {
            DebugConsole.text += "\n" + s;
        }
    }


    private void OnDestroy()
    {
        Input.location.Stop();
    }

    /// <summary>
    /// This function will take two GPS coordinates and return the difference vector in meters.
    /// </summary>
    /// <param name="lat1">Latitude 1</param>
    /// <param name="lon1">Longtidude 1</param>
    /// <param name="lat2">Latitude 2</param>
    /// <param name="lon2">Longitude 2</param>
    /// <returns>Vector 3 global Unity Vector</returns>
    Vector3 VectorBetweenGPS(float lat1, float lon1, float lat2, float lon2)
    {
        //Length in meters of 1° of latitude = always 111.32 km
        //Length in meters of 1° of longitude = 40075 km* cos(latitude ) / 360
        float dLat = ((lat2 - lat1) * 111.32f) * 1000;
        float dLon = (
            ((lon2) * 111.32f * Mathf.Cos(lat2 * Mathf.PI / 180))  //Don't know where the number 40075 came from.
            - ((lon1) * 111.32f * Mathf.Cos(lat1 * Mathf.PI / 180))
            ) * 1000;
        CurrentHeading = Mathf.Atan2(dLat, dLon);
        Vector3 result = new Vector3(dLon, 0.0f, dLat);
        writeToScreenConsole("Resulting Vector = " + result.ToString("F5"));
        return result;
    }

    /// <summary>
    /// This function will return the distance betweent two GPS Points.
    /// </summary>
    /// <param name="lat1">Latitude 1</param>
    /// <param name="lon1">Longtidude 1</param>
    /// <param name="lat2">Latitude 2</param>
    /// <param name="lon2">Longitude 2</param>
    /// <returns>Distance between the two points. </returns>
    float DistanceBetweenGPS(float lat1, float lon1, float lat2, float lon2)
    {
        return VectorBetweenGPS(lat1, lon1, lat2, lon2).magnitude;
    }

    /// <summary>
    /// This function will take a global world coordinate (unity space)
    /// and convert it into a GPS position.   This is based on the initial position of the 
    /// object containing this script and the first GPS reading.
    /// </summary>
    /// <param name="pos">Global Unity Coordinate</param>
    /// <returns>Vector 2 (Lat, Long)</returns>
    Vector2 ToGPS(Vector3 pos)
    {
        if(StartingGPS != null)
        {
            float DifLong = (pos.x - this.transform.position.x)/111320;
            float DifLat =  (pos.z - this.transform.position.z)/111320;
            return StartingGPS + new Vector2(DifLat, DifLong);
        }
        else
        {
            return Vector2.zero;
        }
    }

    /// <summary>
    /// From GPS will convert GPS to a Unity World Coordinate based on the location of the object containing this script.
    /// </summary>
    /// <param name="lat">Lattiude of the object</param>
    /// <param name="lon">Longitude of the object</param>
    /// <returns>Unity World Coordinate</returns>
    Vector3 FromGPS(float lat, float lon)
    {
        return VectorBetweenGPS(lat, lon, StartingGPS.x, StartingGPS.y)+this.transform.position;
    }

    /// <summary>
    /// RunLocationService - Will start the GPS location update.
    /// Written by David Kozdra and Andrew Hanzlik
    /// </summary>
    /// <returns></returns>
    private IEnumerator RunLocationService()
    {
        if (!Input.location.isEnabledByUser)
        {
            Debug.Log("User does not have GPS");
            yield break;
        }
        Input.location.Start(Accuracy, DelatDistance);
        int maxWait = 20;
        while (Input.location.status == LocationServiceStatus.Initializing && maxWait > 0)
        {
            yield return new WaitForSeconds(1);
            maxWait--;
        }
        if (maxWait <= 0)
        {
            Debug.Log("Timed Out");
            yield break;
        }
        GPSStarted = true;
        writeToScreenConsole("GPS Should Have Started!");
    }

    public IEnumerator SampleGPS()
    {
        Vector2 AveragedGPS = new Vector2();
        Vector2 LastSample = new Vector2();
        int i = 0;
        while(i < SampleSize)
        {
            Vector2 TempV = new Vector2(Input.location.lastData.latitude, Input.location.lastData.longitude);
            if(TempV!= LastSample)
            {
                LastSample = TempV;
                AveragedGPS += LastSample;
                i++;
            }
            else
            {
                yield return new WaitForSeconds(.1f);
            }
        }
        CurrentGPSPosition = AveragedGPS / SampleSize;
        writeToScreenConsole("GPS is " + StartingGPS.ToString("F5"));
    }
 

    /// <summary>
    /// This will wait for the GPS to initlize.  
    /// Collect the first value and then keep collecting GPS values until the user has moved half a meter away.
    /// At which point the direction will be set, and the rotation of this object will be inversed.  
    /// This will allow objets childed to this object to have the same global coordinate frame as the GPS frame.
    /// Will also invoke the OnGPSModuleStart event.
    /// </summary>
    /// <returns>Delays for IENumerators</returns>
    private IEnumerator SlowUpdate()
    {
        yield return StartCoroutine(RunLocationService());
        yield return StartCoroutine(SampleGPS());
        StartingGPS = CurrentGPSPosition;
        writeToScreenConsole("Initial GPS is " + StartingGPS.ToString("F5"));

        float tdist = 0;
        Vector3 tdirection;
        do
        {
            yield return new WaitForSeconds(.1f);
            yield return StartCoroutine(SampleGPS());
            tdirection = VectorBetweenGPS(CurrentGPSPosition.x, CurrentGPSPosition.y, StartingGPS.x, StartingGPS.y);
            tdist = tdirection.magnitude;
        } while (tdist < 5f);
        StartingDirection = CurrentHeading;
        writeToScreenConsole("Starting Direction is " + (180*StartingDirection/Mathf.PI));
        GPS_Records.Enqueue(StartingGPS);
        Altitude.Enqueue(Input.location.lastData.altitude);
        //This does not seem to work for some reaon.... maybe I should inverse rotate the playspace.
        //this.transform.rotation = Quaternion.Euler(0, -1 * StartingDirection, 0);
        GPSModuleInitialized = true;

        if(OnGPSModuleStart != null)
        {
            OnGPSModuleStart();
        }
        writeToScreenConsole("GPS Module Initialized!");
        //Start Main Polling Loop.
        while(true)
        {
            Vector2 lastRecord = GPS_Records.ToArray()[GPS_Records.Count - 1];
            yield return StartCoroutine(SampleGPS());
            Vector2 newGPS = CurrentGPSPosition;
            TotalDistance += DistanceBetweenGPS(newGPS.x, newGPS.y, lastRecord.x, lastRecord.y);
            GPS_Records.Enqueue(newGPS);
            Altitude.Enqueue(Input.location.lastData.altitude);
            if (GPS_Records.Count > QueueSize)
            {
                GPS_Records.Dequeue();
                Altitude.Dequeue();
            }
            
            yield return new WaitForSeconds(.1f);
        }

    }


    /// <summary>
    /// This function will create a game object in Unity based on a GPS coordinates.
    /// It requires the module to be initialized, otherwise it will throw an exception.
    /// This will return the game object created.
    /// Note: ALL GAME OBJECTS CREATED BY GPS MUST BE CHILDED TO THE OBJECT WITH THIS SCRIPT.
    /// </summary>
    /// <param name="prefab">The object to be created.</param>
    /// <param name="GPS">The GPS location representing the position of the object</param>
    /// <param name="rotation">Initial rotation of the object.</param>
    /// <returns>The instantiated object</returns>
    public  GameObject CreatObjectFromGPS(GameObject prefab, Vector2 GPS, Quaternion rotation = new Quaternion())
    {
        if(GPSModuleInitialized == false)
        {
            throw new System.Exception("GPS Module is not ready, please wait until GPSModuleInitialized is set to true!");
        }
        GameObject temp = Instantiate(prefab, this.transform);
        temp.transform.localPosition = VectorBetweenGPS(GPS.x, GPS.y, StartingGPS.x, StartingGPS.y);
        temp.transform.rotation = rotation;
        return temp;
    }


    void Update()
    {
        //This is bad...
        //Vector3 Dest = FromGPS(CurrentGPSPosition.x, CurrentGPSPosition.y) + new Vector3(0,Camera.main.transform.position.y,0);
        //Camera.main.transform.position = Vector3.Lerp(Camera.main.transform.position, Dest, 5 * Time.deltaTime);
    }
}
