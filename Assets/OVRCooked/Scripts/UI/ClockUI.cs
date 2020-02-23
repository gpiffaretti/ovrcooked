using UnityEngine;
using UnityEngine.UI;

public class ClockUI : MonoBehaviour
{
    [SerializeField]
    TimeBar timeBar;

    [SerializeField]
    Text timeLeft; 

    GameManager gameManager;

    float totalTime;
    float currentTime;
    float timeRefreshRate = 1f;

    void Awake()
    {
        gameManager = FindObjectOfType<GameManager>();
        gameManager.GameStarted += OnGameStartedHandler;
        gameManager.GameEnded += OnGamePausedHandler;
    }

    // Start is called before the first frame update
    void Start()
    {  
    }

    // Update is called once per frame
    void Update()
    {  
    }

    void OnGameStartedHandler(float time) 
    {
        totalTime = currentTime = time; 
        timeBar.InitTimebar(time);
        InvokeRepeating(nameof(UpdateTimeLeft), 0.0f, timeRefreshRate);
    }

    void UpdateTimeLeft()
    {
        currentTime -= timeRefreshRate;
        int timeMinutes = (int)currentTime / 60;
        int timeSeconds = (int)currentTime % 60;
        timeLeft.text = string.Format("{0:00}:{1:00}", timeMinutes, timeSeconds);
    }

    void OnGamePausedHandler()
    {
    }

    void OnGameEndedHandler()
    {
        CancelInvoke(nameof(UpdateTimeLeft));
    }
}
