using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameUIManager : Singleton<GameUIManager>
{
    [SerializeField]
    IngredientIcon[] ingredientIcons;

    [SerializeField]
    CookwareIcon[] cookwareIcons;

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

    public Sprite GetCookwareIcon(CookwareType cookware)
    {
        foreach (CookwareIcon cookwareIcon in cookwareIcons)
        {
            if (cookwareIcon.cookware == cookware)
                return cookwareIcon.icon;
        }

        throw new Exception($"Icon not found for ingredient => {cookware}");
    }
}

[Serializable]
public class IngredientIcon 
{
    public IngredientType ingredient;
    public Sprite icon;
}

[Serializable]
public class CookwareIcon
{
    public CookwareType cookware;
    public Sprite icon;
}