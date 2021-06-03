using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;
using UnityEngine.Networking;

public class GameOverController : MonoBehaviour
{
    public GameObject scoreboardCanvas;
    public TMP_Text finalText;
    public GameObject loadingScreen;
    public GetUserData getuserData;
    private bool scoreDisplay = false;

    private void Start()
    {
        GameDataDisplay();
        StartCoroutine(GetUserData());
    }

    IEnumerator GetUserData()
    {
        yield return StartCoroutine(getuserData.GetUsers("http://mail.balvurcb.lv:8080/lietotaji.php"));
        loadingScreen.SetActive(false);
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

    public void GameDataDisplay()
    {
        finalText.text = MenuScreenController.username + ", Tavs izpildīto pilsētu skaits ir " + "<color=orange>" + GameManager.buttonCount + "</color>" +" un kopējais nopelnītais punktu skaits ir " + "<color=orange>" + GameManager.allScore + "</color>" + "<br>" + "<br>" + "Kopumā tika atbildēts uz " + "<color=orange>" + GameManager.questionCount + "</color>" +" jautājumiem no kuriem " + "<color=green>" + GameManager.correctAnswerCount + "</color>" + " bija pareizi un " + "<color=red>" + GameManager.incorrectAnswerCount + "</color>" +" bija nepareizi";
    }

    public void PlayAgain()
    {
        GameManager.buttonCount = 0;
        GameManager.allScore = 0;
        GameManager.questionCount = 0;
        GameManager.correctAnswerCount = 0;
        GameManager.incorrectAnswerCount = 0;
        SceneManager.LoadScene("MenuScreen");
    }

    public void DataBase()
    {
        Application.OpenURL("");
    }

    public void ReturnToMenu()
    {
        scoreboardCanvas.SetActive(false);
    }
}
