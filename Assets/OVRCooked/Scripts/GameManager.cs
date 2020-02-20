using Assets.OVRCooked.Scripts.Orders;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField]
    OrderManager orderManager;

    public event Action GameStarted;
    public event Action GameEnded;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void StartGame() 
    {
        // start all subsystems
        orderManager.StartSpawning();

        // notify
        GameStarted?.Invoke();
    }

    public void EndGame() 
    {
        // set all subsystems to paused or finished state

        // notify
        GameEnded?.Invoke();
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
