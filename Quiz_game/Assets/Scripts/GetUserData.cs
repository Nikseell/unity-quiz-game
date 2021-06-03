using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GetUserData : MonoBehaviour
{
    public List<Users> userList = new List<Users>();
    public GameObject noPlayers;
    public TMP_Text noPlayersText;
    public GameObject prefab;
    public Transform prefabPanel;
    private int listCount;
    private int size;
    public bool playerLimit;

    public IEnumerator GetUsers(string userurl)
    {
        var userdata = WebAPI.GetUserDataLink(userurl);
        yield return userdata.SendWebRequest();
        var users = WebAPI.HandleUserData(userdata);
        if (users == null)
        {
            yield break;
        }
        Debug.Log("UserData recived");
        yield return StartCoroutine(InputUserDataIntoList(users));
    }

    public IEnumerator InputUserDataIntoList(UserData users)
    {
        foreach (var user in users.Users)
        {
            userList.Add(user);
        }
        yield return new WaitForEndOfFrame();
    }

    public void ShowUsers()
    {
        if (userList.Count < 1)
        {
            noPlayersText.text = "Spēlētāju rezultāti tiks parādīti, ja būs vismaz viens spēlētājs";
            noPlayers.SetActive(true);
        }
        if (userList.Count <= 10)
        {
            size = userList.Count;
        }
        else
        {
            PlayerLimit();
        }
        for (int i = 0; i < size; i++)
        {
            listCount++;
            GameObject clone = Instantiate(prefab);
            clone.transform.SetParent(prefabPanel.transform, false);
            ScoreBoardEntry entry = clone.GetComponent<ScoreBoardEntry>();
            foreach (var user in userList)
            {
                entry.place.text = listCount.ToString();
                entry.username.text = userList[i].lietotajvards;
                entry.placeCount.text = userList[i].pilsetuskaits.ToString();
                entry.score.text = userList[i].punktuskaits.ToString();
            }
        }
    }

    public void PlayerLimit()
    {
        if (playerLimit)
        {
            size = 10;
        }
        else
        {
            size = userList.Count;
        }
    }
}
