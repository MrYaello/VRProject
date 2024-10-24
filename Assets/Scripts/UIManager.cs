using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class UIManager : MonoBehaviour
{
    public static UIManager instance;
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(this.gameObject);
        }
    }

    GameController gameController;
    Pooler pooler;
    int score;

    [Header("Timer variables")]
    float timer;
    bool canCount = false;
    int minutes;
    int seconds;
    string timeStr;

    public int lives = 3;

    [Header("UI variables")]
    public TextMeshProUGUI timeText;
    public TextMeshProUGUI scoreText;
    public CanvasGroup GameplayPanel;
    public CanvasGroup FinishPanel;
    public TextMeshProUGUI FinishScore;
    public GameObject firstHeart;
    public GameObject secondHeart;
    public GameObject thirdHeart;

    [Header("Menu variables")]
    public GameObject StartArcadeModeButton;

    private void Start()
    {
        gameController = GameController.instance;
        pooler = Pooler.instance;
        GameplayPanel.alpha = 0;
        FinishPanel.alpha = 0;
    }

    private void Update()
    {
        if (lives > 0 && canCount)
        {
            timer += Time.deltaTime;
            minutes = Mathf.FloorToInt(timer / 60f);
            seconds = Mathf.FloorToInt(timer - minutes * 60);
            timeStr = string.Format("{0:0} : {1:00}", minutes, seconds);
            timeText.text = timeStr;
        }
        else if (lives <= 0 && canCount)
        {
            Finish();
        }
    }

    public void StartArcadeMode()
    {
        StartArcadeModeButton.SetActive(false);
        FinishPanel.alpha = 0;
        GameplayPanel.alpha = 1;
        lives = 3;
        firstHeart.SetActive(true);
        secondHeart.SetActive(true);
        thirdHeart.SetActive(true);
        score = 0;
        timer = 0f;
        canCount = true;
        timeText.text = " 0 : 00";
        scoreText.text = "0";
        pooler.StartGameInPool(0);
    }

    public void DecreaseHealth()
    {
        lives -= 1;
        switch (lives)
        {
            case 0:
                firstHeart.SetActive(false); break;
            case 1:
                secondHeart.SetActive(false); break;
            case 2:
                thirdHeart.SetActive(false); break;
            default: break;
        }
    }

    public void IncreaseScore(int count)
    {
        score += count;
        scoreText.text = score.ToString();
    }
    public void Finish()
    {
        pooler.ResetPool();
        GameplayPanel.alpha = 0;
        FinishPanel.alpha = 1;
        FinishScore.text = score.ToString();

        StartArcadeModeButton.SetActive(true);

        canCount = false;
        timeText.text = "0 : 00";
    }

    public void BeforeSelectButton()
    {
        foreach (Transform item in transform)
        {
            item.gameObject.SetActive(false);
        }
    }

    public void SelectButton(string _fruitParentButton)
    {
        switch (_fruitParentButton)
        {
            case "Arcade":
                StartArcadeMode();
                break;
            default:
                Debug.Log("Please Check your Button's name!");
                break;
        }
    }
}


