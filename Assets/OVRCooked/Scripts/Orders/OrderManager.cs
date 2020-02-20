using Assets.OVRCooked.Scripts.Orders;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class OrderManager : Singleton<OrderManager>
{
    public event Action<Order> OrderCreated;

    [SerializeField]
    Recipe[] recipes;

    [SerializeField]
    int maxConcurrentOrders = 3;

    [SerializeField]
    [Range(10,60)]
    float waitForSpawningSeconds = 30;
    [SerializeField]
    [Range(30, 180)]
    float recipeTimeSeconds = 45;

    List<Order> activeOrders = new List<Order>();

    private bool isPlaying;
    private Coroutine spawnCoroutine;
    
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (isPlaying) // we can also disable the component to avoid this when not playing 
        {
            for (int i = 0; i < activeOrders.Count; i++)
            {
                activeOrders[i].UpdateOrderTime(Time.deltaTime);
            }
        }
        
    }

    public void StartSpawning() 
    {
        isPlaying = true;
        spawnCoroutine = StartCoroutine(SpawnOrders()); ;
    }

    public void StopSpawning()
    {
        isPlaying = false;
        StopCoroutine(spawnCoroutine);
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

    private void SortOrdersByTime() 
    {
        activeOrders.Sort((o1, o2) => o1.TimeLeft.CompareTo(o2.TimeLeft));
    }

    public bool AttemptDelivery(Plate plate, out Order order) 
    {
        IngredientType[] plateContent = plate.Content;

        SortOrdersByTime();
        for (int i = 0; i < activeOrders.Count; i++)
        {
            IngredientType[] recipeSpecification = activeOrders[i].recipe.ingredients;
            if (IngredientMatch(plateContent, recipeSpecification)) 
            {
                order = activeOrders[i];
                activeOrders.RemoveAt(i);
                order.DeliverOrder();
                return true;
            }
        }
        order = null;
        return false;
    }

    private bool IngredientMatch(IngredientType[] ing1, IngredientType[] ing2) 
    {
        if (ing1.Length != ing2.Length) return false;

        // we want to flag the elements in the second array that have been already been matched with one in the first array
        bool[] matchedIngredients = new bool[ing2.Length];

        // for each ingredient in first array, look for a match in the second array
        for (int i = 0; i < ing1.Length; i++)
        {
            IngredientType currentIngredient = ing1[i];

            for (int j = 0; j < ing2.Length; j++)
            {
                if (currentIngredient == ing2[j]) 
                {
                    matchedIngredients[j] = true;
                    break;
                }
            }
        }

        // if any of the ingredients of the second array wasn't matched, return false, otherwise it's a match
        return matchedIngredients.Any(m => !m);
        
        // TODO: can be improved so that it returns false exactly after one element wasn't found... no need to keep matching.
    }

    Recipe GetRandomRecipe()
    {
        return recipes[UnityEngine.Random.Range(0, recipes.Length-1)];
    }
}
