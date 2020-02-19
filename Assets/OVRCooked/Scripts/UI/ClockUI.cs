using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClockUI : MonoBehaviour
{

    [SerializeField]
    TimeBar timeBar;

    // Start is called before the first frame update
    void Start()
    {
        //GameManager gameManger = FindObjectOfType<GameManager>();
        //gameManger.GameStarted += OnGameStartedHandler;
        //gameManger.GameEnded += OnGamePausedHandler;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnGameStartedHandler() 
    {
        // init timebar
    }

    void OnGameEndedHandler()
    {
        // init timebar
    }
}
