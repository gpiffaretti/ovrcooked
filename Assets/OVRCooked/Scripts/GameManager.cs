using Assets.OVRCooked.Scripts.Orders;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    private int points;

    [SerializeField]
    OrderManager orderManager;

    public event Action<float> GameStarted;
    public event Action GamePaused; // To use in the future
    public event Action GameEnded;
    public event Action<int> PointsChanged;

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
        StartCoroutine(IncreasePointsRandomly()); // Workaround to visualize points changing

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

    private void AddPoints(int points)
    {
        var result = this.points + points;
        if (result < 0)
            this.points = 0;
        else
            this.points = result;

        PointsChanged?.Invoke(this.points);
    }

    private IEnumerator IncreasePointsRandomly()
    {
        while(true)
        {
            AddPoints(100);
            yield return new WaitForSeconds(2.0f);
        }
    }

    internal void DeliverPlate(Plate plate)
    {
        Order matchedOrder;
        if (orderManager.AttemptDelivery(plate, out matchedOrder))
        {
            // success! add score based on (completed order + time left)
            Debug.Log($"Delivery complete with {matchedOrder.TimeLeft} seconds left!!!");
            AddPoints(100);
        }
        else
        {
            // plate didn't match any order!
            // penalize score
            Debug.Log("Delivery didn't match any order!!!");
            AddPoints(-100);
        }
    }
}
