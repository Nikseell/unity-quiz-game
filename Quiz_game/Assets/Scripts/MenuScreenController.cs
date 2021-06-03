using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class MenuScreenController : MonoBehaviour
{
    [Header("InputField")]
    public static string username;
    public TMP_InputField inputField;
    public GameObject incorrectInput;
    public TMP_Text incorrectText;
    [Header("Canvas")]
    [Space]
    public GameObject usernameCanvas;
    public GameObject scoreboardCanvas;
    public GameObject aboutGameCanvas;
    [Header("ScoreBoard Canvas")]
    [Space]
    public GetUserData getuserData;
    public GameObject loadingScreen;
    private bool scoreDisplay = false;

    private void Start()
    {
        StartCoroutine(GetUserData());
    }

    IEnumerator GetUserData()
    {
        yield return StartCoroutine(getuserData.GetUsers("http://mail.balvurcb.lv:8080/lietotaji.php"));
        loadingScreen.SetActive(false);
    }

    public void StartGame()
    {
        usernameCanvas.SetActive(true);
    }
    public void ScoreBoardDisplay()
    {
        scoreboardCanvas.SetActive(true);
        if (scoreDisplay == false)
        {
            getuserData.ShowUsers();
            scoreDisplay = true;
        }
    }

    public void AboutGame()
    {
        aboutGameCanvas.SetActive(true);
    }

    public void UsernameInput()
    {
        if (inputField.text.Length < 3)
        {
            incorrectInput.SetActive(true);
            incorrectText.text = "Lietotājvārdam jāsastāv vismaz no 3 burtiem";
        }
        else
        {
            username = inputField.text;
            SceneManager.LoadScene("Game");
        }
    }

    public void ReturnToMenu()
    {
        scoreboardCanvas.SetActive(false);
        usernameCanvas.SetActive(false);
        aboutGameCanvas.SetActive(false);
    }
}
