using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Networking;
using System;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    [Header("Canvas")]
    public GameObject quizCanvas;
    public GameObject mapCanvas;
    public GameObject roundOverPanel;
    [Header("Quiz Canvas")]
    [Space]
    public TMP_Text questionDisplayText;
    public TMP_Text button1Text;
    public TMP_Text button2Text;
    public TMP_Text button3Text;
    public TMP_Text button4Text;
    public TMP_Text roundOverScoreText;
    public TMP_Text answerCountText;
    public GameObject loadingScreen;
    public GameObject skipButton;
    [Header("MapCanvas")]
    [Space]
    public Button[] mapButtons;
    public GameObject returnToMapButton;
    public GameObject gameOverButton;
    public static int buttonCount;
    [Header("Correct or Incorrect Panel")]
    [Space]
    public GameObject panel;
    public GameObject infoPanel;
    public Image statePanel;
    public TMP_Text infoText;
    public TMP_Text stateText;
    public GameObject stateButton;
    public GameObject roundOverButton;
    public GameObject infoButton;
    public GameObject moreInfoButton;
    public Sprite correctBG;
    public Sprite incorrectBG;
    public static string infoLink = "";
    [Header("Timer and Score")]
    [Space]
    public GameObject timer;
    public TMP_Text timeRemainingDisplayText;
    public TMP_Text allScoreDisplayText;
    public static int allScore;
    private int playerScore;
    private float timeRemaining;
    private int correctAnswer;
    private int incorrectAnswer;
    private int question;
    public static int questionCount;
    public static int correctAnswerCount;
    public static int incorrectAnswerCount;
    private readonly int pointsAddedForCorrectAnswer = 10;
    [Header("AnswerButtons")]
    [Space]
    public GameObject answerButton2;
    public GameObject answerButton3;
    public GameObject[] answerButtons;
    [Header("Question Data")]
    [Space]
    public List<Results> resultsList = new List<Results>();
    private bool isQuestionActive;
    public Animator animator;

    private void Start()
    {
       animator.SetTrigger("FallAnimation");
    }

    private void Awake()
    {
        instance = this;
    }

    public IEnumerator Load(string url)
    {
        yield return StartCoroutine(GetQuestion(url));
        loadingScreen.SetActive(false);
    }

    public IEnumerator GetQuestion(string url)
    {
        var req = WebAPI.CreateReturnPlayerDataRequest(url);
        yield return req.SendWebRequest();
        var results = WebAPI.HandleReturnPlayerDataRequest(req);

        if (results == null)
            yield break;

        Debug.Log("Data recieved");
        yield return StartCoroutine((InputQuestionsInList(results)));  
    }
    public IEnumerator InputQuestionsInList(Questions results)
    {
        foreach (var result in results.Result)
        {
            resultsList.Add(result);
        }
        yield return new WaitForEndOfFrame();
        NextQuestion();
    }

    public void PlaceClicked(string url)
    {
        StartCoroutine(Load(url));
        skipButton.SetActive(true);
        quizCanvas.SetActive(true);
        mapCanvas.SetActive(false);
        timer.SetActive(true);
        playerScore = 0;
        correctAnswer = 0;
        incorrectAnswer = 0;
        question = 0;
    }

    public void ButtonClick(int value)
    {
        buttonCount++;
        mapButtons[value].image.color = Color.green;
        mapButtons[value].interactable = false;
    }

    public void NextQuestion()
    {
        panel.SetActive(false);
        RandomOrder();
        questionDisplayText.text = resultsList[0].question;
        button1Text.text = resultsList[0].answer1;
        button2Text.text = resultsList[0].answer2;
        button3Text.text = resultsList[0].answer3;
        button4Text.text = resultsList[0].answer4c;
        infoText.text = resultsList[0].info;
        infoLink = resultsList[0].link;

        infoButton.SetActive(!string.IsNullOrEmpty(resultsList[0].info));
        moreInfoButton.SetActive(!string.IsNullOrEmpty(resultsList[0].link));

        answerButton2.SetActive(!string.IsNullOrEmpty(resultsList[0].answer2));
        answerButton3.SetActive(!string.IsNullOrEmpty(resultsList[0].answer3));

        timeRemaining = 60;
        isQuestionActive = true;
        UpdateTimeRemainingDisplay();
        question++;
        if (resultsList.Count == 1)
        {
            skipButton.SetActive(false);
        }
    }

    public void RemoveQuestion()
    {
        if(resultsList.Count >= 1)
        {
            resultsList.Remove(resultsList[0]);
        }
    }

    public void QuestionCheck()
    {
        if (resultsList.Count <= 1)
        {
            stateButton.SetActive(false);
            roundOverButton.SetActive(true);
        }
    }
    public void AnswerButtonClicked(int value)
    {
        isQuestionActive = false;
        if (value == 0 )
        {
            Incorrect();
        }
        else
        {
            Correct();
        }
        QuestionCheck();
    }

    public void SkipButton()
    {
        if (resultsList.Count > 1)
        {
            var temp = resultsList[0];
            resultsList.Remove(resultsList[0]);
            resultsList.Add(temp);
            NextQuestion();
        }
    }
    public void Incorrect()
    {
        panel.SetActive(true);
        stateButton.SetActive(true);
        stateText.text = "Atbilde nepareiza!" + "<br>" + "<br>" + "Pareizā atbilde bija: " + "<u>" + resultsList[0].answer4c;
        statePanel.sprite = incorrectBG;
        incorrectAnswer++;
    }

    private void Correct()
    {
        panel.SetActive(true);
        stateButton.SetActive(true);
        stateText.text = "Atbilde pareiza!" + "<br>" + "<br>" + " Pareizā atbilde bija: " + "<u>" + resultsList[0].answer4c;
        statePanel.sprite = correctBG;
        playerScore += pointsAddedForCorrectAnswer;
        correctAnswer++;
    }

    public void InfoButton()
    {
        infoPanel.SetActive(true);
    }

    public void GoBackButton()
    {
        infoPanel.SetActive(false);
    }

    private void RandomOrder()
    {
        foreach (var buttons in answerButtons)
        {
            buttons.transform.SetSiblingIndex(UnityEngine.Random.Range(0, answerButtons.Length));
        }
    }

    public void RoundOver()
    {
        resultsList.Remove(resultsList[0]);
        loadingScreen.SetActive(true);
        AllScore();
        QuestionsData();
        isQuestionActive = false;
        if (buttonCount == 10)
        {
            returnToMapButton.SetActive(false);
            gameOverButton.SetActive(true);
        }
        quizCanvas.SetActive(false);
        panel.SetActive(false);
        roundOverPanel.SetActive(true);

        roundOverScoreText.text = "Šajā pilsētā nopelnītais punktu skaits ir : " + playerScore.ToString();
        answerCountText.text = "Pareizi tika atbildēts uz " + "<color=green>" + correctAnswer + "</color>" + " jautājumiem un uz " + "<color=red>" + incorrectAnswer + "</color>" + " nepareizi!";
        if (incorrectAnswer == 0)
        {
            answerCountText.text = "Pareizi tika atbildēts uz visiem " + "<color=green>" + correctAnswer + "</color>" + " jautājumiem!";
        }
        if (correctAnswer == 1)
        {
            answerCountText.text = "Pareizi tika atbildēts uz " + "<color=green>" + correctAnswer + "</color>" + " jautājumu un uz " + "<color=red>" + incorrectAnswer + "</color>" + " nepareizi!";
        }
        if (incorrectAnswer == 5)
        {
            answerCountText.text = "Nepareizi tika atbildēts uz visiem " + "<color=red>" + incorrectAnswer + "</color>" + " jautājumiem!";
        }
    }

    public void GoToMap()
    {
        mapCanvas.SetActive(true);
        roundOverPanel.SetActive(false);
        roundOverButton.SetActive(false);
    }

    public void FinishGame()
    {
        if (buttonCount >= 1)
        {
            StartCoroutine(SendUserData());
        }
        SceneManager.LoadScene("GameOver");
    }

    private void UpdateTimeRemainingDisplay()
    {
        timeRemainingDisplayText.text = Mathf.Round(timeRemaining).ToString();
        timeRemainingDisplayText.color = Color.white;
        if (Mathf.Round(timeRemaining) <= 30)
        {
            timeRemainingDisplayText.color = Color.yellow;
        }
        if (Mathf.Round(timeRemaining) <= 10)
        {
            timeRemainingDisplayText.color = Color.red;
        }
    }

    public void TimeOut()
    {
       if (resultsList.Count >= 2)
        {
            resultsList.Remove(resultsList[0]);
            incorrectAnswer++;
            NextQuestion();
        }
       if (resultsList.Count == 1)
        {
            incorrectAnswer++;
            RoundOver();
        }
    }

    private void AllScore()
    {
        allScore += playerScore;
        allScoreDisplayText.text = "Kopējais punktu skaits: " + allScore;
    }

    public void QuestionsData()
    {
        questionCount += question;
        correctAnswerCount += correctAnswer;
        incorrectAnswerCount += incorrectAnswer;
    }

    private void Update()
    {
        if (isQuestionActive)
        {
            timeRemaining -= Time.deltaTime;
            UpdateTimeRemainingDisplay();
            if (timeRemaining <= 0)
            {
                TimeOut();
            }
        }
    }

    IEnumerator SendUserData()
    {
        WWWForm form = new WWWForm();
        form.AddField("username", MenuScreenController.username);
        form.AddField("complitedPlaces", buttonCount);
        form.AddField("score", allScore);

        using (UnityWebRequest www = UnityWebRequest.Post("http://mail.balvurcb.lv:8080/lietotajiPOST.php", form))
        {

            yield return www.SendWebRequest();

            if (www.isNetworkError || www.isHttpError)
            {
                Debug.Log(www.error);
            }
            else
            {
                Debug.Log("Form upload complete!");
            }
        }
    }
}
