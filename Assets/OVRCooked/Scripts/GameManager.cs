using Assets.OVRCooked.Scripts.Orders;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField]
    OrderManager orderManager;

    public event Action<float> GameStarted;
    public event Action GamePaused; // To use in the future
    public event Action GameEnded;

    [Range(120, 500)]
    public float gameTimeSeconds = 180;

    SimpleTimer gameTimer;

    // Start is called before the first frame update
    void Start()
    {
        AddTimerComponent();
    }

    private void AddTimerComponent() 
    {
        gameTimer = gameObject.AddComponent<SimpleTimer>();
        gameTimer.SetTimer(gameTimeSeconds);
        gameTimer.Completed += OnGameTimeCompleted;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void StartGame() 
    {
        // start all subsystems
        gameTimer.StartTimer();
        orderManager.StartSpawning();

        // notify
        GameStarted?.Invoke(gameTimeSeconds);
    }

    public void EndGame() 
    {
        // set all subsystems to paused or finished state

        // notify
        GameEnded?.Invoke();
    }

    private void OnGameTimeCompleted() 
    {
        EndGame();    
    }

    internal void DeliverPlate(Plate plate)
    {
        Order matchedOrder;
        if (orderManager.AttemptDelivery(plate, out matchedOrder))
        {
            // success! add score based on (completed order + time left)
            Debug.Log($"Delivery complete with {matchedOrder.TimeLeft} seconds left!!!");
        }
        else
        {
            // plate didn't match any order!
            // penalize score
            Debug.Log("Delivery didn't match any order!!!");
        }
    }
}
