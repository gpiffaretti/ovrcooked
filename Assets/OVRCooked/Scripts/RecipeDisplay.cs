using UnityEngine;
using UnityEngine.UI;

public class RecipeDisplay : MonoBehaviour
{
    public Recipe recipe;

    public Image recipeArtwork;
    public Image[] ingredients;
    public Image[] cookware;

    // Start is called before the first frame update
    void Start()
    {
        recipeArtwork.sprite = recipe.recipeArtwork;
        
        for (int i =0; i < ingredients.Length; i++)
        {
            if (recipe.ingredients[i] == null)
            {
                ingredients[i].enabled = false;
                continue;
            }

            ingredients[i].sprite = recipe.ingredients[i];
        }

        for (int i = 0; i < cookware.Length; i++)
        {
            if (recipe.cookware[i] == null)
            {
                cookware[i].enabled = false;
                continue;
            }

            cookware[i].sprite = recipe.cookware[i];
        }
    }
}
