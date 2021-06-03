using System.Collections;
using UnityEngine;
using UnityEngine.Networking;
using System;

[Serializable]
public class Questions
{
    public Results[] Result;
}

[Serializable]
public class Results
{
    public int id;
    public string question;
    public string answer1;
    public string answer2;
    public string answer3;
    public string answer4c;
    public string info;
    public string link;
}

public class UserData
{
    public Users[] Users;
}
[Serializable]
public class Users
{
    public int id;
    public string lietotajvards;
    public int pilsetuskaits;
    public int punktuskaits;
}

public static class WebAPI
{
    public static UnityWebRequest CreateReturnPlayerDataRequest(string url)
    {
        var req = UnityWebRequest.Get(url);
        return req;
    }

    public static Questions HandleReturnPlayerDataRequest(UnityWebRequest req)
    {
        if (req.isNetworkError)
        {
            Debug.LogError($"Failed to POST /player/register! Failed with {req.responseCode} - Reason: {req.error}");
            return null;
        }

        var results = JsonUtility.FromJson<Questions>("{\"Result\":" + req.downloadHandler.text + "}");
        return results;
    }

    public static UnityWebRequest GetUserDataLink(string userurl)
    {
        var userdata = UnityWebRequest.Get(userurl);
        return userdata;
    }

    public static UserData HandleUserData(UnityWebRequest data)
    {
        if (data.isNetworkError)
        {
            Debug.LogError($"Failed to POST /player/register! Failed with {data.responseCode} - Reason: {data.error}");
            return null;
        }

        var users = JsonUtility.FromJson<UserData>("{\"Users\":" + data.downloadHandler.text + "}");
        return users;
    }
}
