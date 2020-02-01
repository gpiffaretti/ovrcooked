using Assets.OVRCooked.Scripts.Orders;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OrderManager : Singleton<OrderManager>
{
    public event Action<Order> OrderCreated;

    [SerializeField]
    Recipe[] recipes;

    [SerializeField]
    int maxConcurrentOrders = 3;

    float waitForSpawningSeconds = 30;
    float recipeTimeSeconds = 120;

    List<Order> activeOrders = new List<Order>();
    
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(SpawnOrders());
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    IEnumerator SpawnOrders()
    {
        while(true)
        {
            while(activeOrders.Count < maxConcurrentOrders)
            {
                var generatedOrder = new Order(GetRandomRecipe(), recipeTimeSeconds);
                generatedOrder.OrderExpired += OnOrderExpired;
                activeOrders.Add(generatedOrder);
                OrderCreated?.Invoke(generatedOrder);

                yield return new WaitForSeconds(3);
            }

            yield return new WaitForSeconds(waitForSpawningSeconds);
        }
    }

    private void OnOrderExpired(Order order)
    {
        activeOrders.Remove(order);
    }

    Recipe GetRandomRecipe()
    {
        return recipes[UnityEngine.Random.Range(0, recipes.Length-1)];
    }
}
