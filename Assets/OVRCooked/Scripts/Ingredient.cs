using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ingredient : MonoBehaviour
{
    [SerializeField]
    IngredientType type;

    public IngredientType Type { get { return type; } }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

[Serializable]
public enum IngredientType 
{
    Onion,
    Carrot,
    Tomato
}

[Serializable]
public enum CookwareType
{
    Pot,
    FryingPan
}