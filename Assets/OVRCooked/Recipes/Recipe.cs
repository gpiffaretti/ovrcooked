using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName ="New Recipe", menuName ="Recipe")]
public class Recipe : ScriptableObject
{
    public new string name;
    public int secondsToCook;

    public Sprite recipeArtwork;
    public Sprite[] ingredients;
    public Sprite[] cookware;
}
