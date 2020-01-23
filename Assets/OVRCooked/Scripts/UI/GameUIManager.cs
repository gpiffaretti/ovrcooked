using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameUIManager : Singleton<GameUIManager>
{

    [SerializeField]
    IngredientIcon[] ingredientIcons;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    public Sprite GetIngredientIcon(IngredientType ingredient) 
    {
        foreach (IngredientIcon ingredientIcon in ingredientIcons)
        {
            if (ingredientIcon.ingredient == ingredient)
                return ingredientIcon.icon;
        }

        throw new Exception($"Icon not found for ingredient => {ingredient}");
    }

}

[Serializable]
public class IngredientIcon 
{
    public IngredientType ingredient;
    public Sprite icon;
}