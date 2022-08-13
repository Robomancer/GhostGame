using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public static class JsonHelper
{
    public static List<T> FromJson<T>(string json)
    {
        Wrapper<T> wrapper = JsonUtility.FromJson<Wrapper<T>>(json);
        return wrapper.result;
    }

    [System.Serializable]
    private class Wrapper<T>
    {
        public List<T> result;
    }
}

[System.Serializable]
public class Item{

    public string name = "";
    public GameObject Prefab;

    public Item(string name){
        this.name = name;
    }
}


[System.Serializable]
public class User{

    public string name = "";
    public string ID = "";

    public User(string name, string ID){
        this.name = name;
        this.ID = ID;
    }
}


public class ClientAPI : MonoBehaviour
{
    public string url;

    void Start()
    {
        StartCoroutine(Get(url+"/users  "));
        
        //StartCoroutine(Post(url+"/users/add", new User("Name", "ID")));
    }

    public IEnumerator Get(string url)
    {
        using(UnityWebRequest www = UnityWebRequest.Get(url)){
            yield return www.SendWebRequest();

            if (www.result == UnityWebRequest.Result.ConnectionError)
            {
                Debug.Log(www.error);
            }
            else
            {
                if (www.isDone)
                {
                    // handle the result
                    var result = System.Text.Encoding.UTF8.GetString(www.downloadHandler.data);
                    Debug.Log(result);
                }
                else
                {
                    //handle the problem
                    Debug.Log("Error! data couldn't get.");
                }
            }
        }

    }

public IEnumerator Post(string url, Item item)
{
  var jsonData = JsonUtility.ToJson(item);
  Debug.Log(jsonData);

  using(UnityWebRequest www = UnityWebRequest.Post("localhost:5000/users/add", jsonData))
  {
        www.SetRequestHeader("content-type", "application/json");
    www.uploadHandler.contentType = "application/json";
    www.uploadHandler = new UploadHandlerRaw(System.Text.Encoding.UTF8.GetBytes(jsonData));
    yield return www.SendWebRequest();

    if (www.result == UnityWebRequest.Result.ConnectionError)
    {
      Debug.Log(www.error);
    }
    else
    {
      if (www.isDone)
      {
        // handle the result
        var result = System.Text.Encoding.UTF8.GetString(www.downloadHandler.data);  
        result = "{\"result\":" + result + "}"; 
        Debug.Log(result);
      }
      else
      {
        //handle the problem
        Debug.Log("Error! data couldn't get.");
      }
    }
  }
}
}